using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaApi.Data;
using SocialMediaApi.Models;

namespace SocialMediaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthenticationController(DataContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpPost]
        [Route("/login")]
        public async Task<ActionResult<string>> LoginUser(string username, string password)
        {
            User user;
            if (new EmailAddressAttribute().IsValid(username) && UserExists(new MailAddress(username)))
            {
                user = _context.Users.Where(x => x.Email == username).First();
            }
            else if (UserExists(username))
            {
                user = _context.Users.Where(x => x.Username == username).First();
            }
            else
            {
                return NotFound();
            }

            //  Build the token value
            StringBuilder builder = new StringBuilder(LoginToken.ValueLength);
            Random random = new Random();
            string tokenValue;
            while (true)
            {
                for (int i = 0; i < LoginToken.ValueLength; i++)
                {
                    char c = LoginToken.AllowedCharacters[random.Next(LoginToken.AllowedCharacters.Length)];
                    builder.Append(c);
                }
                tokenValue = builder.ToString();

                if (!TokenExists(tokenValue))
                {
                    break;
                }
            }

            //  Create the token object
            LoginToken token = new LoginToken
            {
                Owner = user,
                HashedValue = StringHasher.HashString(tokenValue),
                LastAccessed = DateTime.Now,
                DeviceIdiom = "",
                DeviceManufacturer = "",
                DeviceModel = "",
                DevicePlatform = "", 
                DeviceType = ""
            };

            try
            {
                await _context.LoginTokens.AddAsync(token);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                return BadRequest(e.Message);
            }
            return tokenValue;

        }

        [HttpPost]
        [Route("/register")]
        public async Task<ActionResult<string>> RegisterUser(UserRegistrationInput input)
        {
            if (!UserExists(input.Username) && !UserExists(new MailAddress(input.Email)) && new EmailAddressAttribute().IsValid(input.Email))
            {
                User user = new User
                {
                    Username = input.Username,
                    Email = input.Email,
                    ProfilePictureSource = "/Images/Default/pfp.jpg"
                };
                user.HashedPassword = _passwordHasher.HashPassword(user, input.Password);

                try
                {
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    return BadRequest(e.Message);
                }

                return await LoginUser(input.Username, input.Password);
            }
            else
            {
                return BadRequest();
            }
        }

        // GET: api/Authentication
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        //{
        //    return await _context.Users.ToListAsync();
        //}

        // GET: api/Authentication/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user;
        //}

        // PUT: api/Authentication/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(int id, User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Authentication
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(User user)
        //{
        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.Id }, user);
        //}

        // DELETE: api/Authentication/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private bool UserExists(string username)
        {
            return _context.Users.Any(e => e.Username.Equals(username));
        }

        private bool UserExists(MailAddress email)
        {
            return _context.Users.Any(e => e.Email.Equals(email.Address));
        }

        private bool TokenExists(string token)
        {
            return _context.LoginTokens.Any(e => e.HashedValue.Equals(StringHasher.HashString(token)));
        }
    }
}
