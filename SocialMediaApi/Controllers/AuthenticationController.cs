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
using SocialMediaApi.Models.Helpers;
using SocialMediaApi.Models.Input;
using SocialMediaApi.Models.Output;

namespace SocialMediaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DataContext _context;

        public AuthenticationController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("/Authenticate")]
        public async Task<ActionResult<AuthenticatedUserOutput>> AuthenticateUserToken(UserAuthorizationInput input)
        {
            if (!TokenExists(input.LoginToken))
            {
                return NotFound();
            }
            
            LoginToken? token = await _context.LoginTokens.Where(x => x.HashedValue.Equals(StringHasher.HashString(input.LoginToken))).Include(x => x.Owner).Include(x => x.Owner.Followers).FirstAsync();
            if (token == null) return NotFound();
            else if (
                token.DevicePlatform.Equals(input.DevicePlatform) &&
                token.DeviceIdiom.Equals(input.DeviceIdiom) && 
                token.DeviceType.Equals(input.DeviceType) &&
                token.DeviceModel.Equals(input.DeviceModel) &&
                token.DeviceManufacturer.Equals(input.DeviceManufacturer) && 
                (DateTime.Now - token.LastAccessed).TotalDays < 60)
            {
                User user = token.Owner;
                AuthenticatedUserOutput output = new AuthenticatedUserOutput
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    ProfilePictureSource = user.ProfilePictureSource,
                    FollowerCount = user.Followers.Count
                };

                try
                {
                    token.LastAccessed = DateTime.Now;
                    _context.LoginTokens.Update(token);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    Console.WriteLine(e.Message);
                }

                return output;
            }
            else if ((DateTime.Now - token.LastAccessed).TotalDays > 60)
            {
                try
                {
                    _context.LoginTokens.Remove(token);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return BadRequest("Token mismatched or expired");
        }

        [HttpPost]
        [Route("/login")]
        public async Task<ActionResult<string>> LoginUser(UserLoginInput input)
        {
            User user;
            if (new EmailAddressAttribute().IsValid(input.Username) && UserExists(new MailAddress(input.Username)))
            {
                user = _context.Users.Where(x => x.Email == input.Username).First();
            }
            else if (UserExists(input.Username))
            {
                user = _context.Users.Where(x => x.Username == input.Username).First();
            }
            else
            {
                return NotFound();
            }
            if (user.HashedPassword != StringHasher.HashString(input.Password))
            {
                return BadRequest("Incorrect Password");
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
                DeviceIdiom = input.DeviceIdiom,
                DeviceManufacturer = input.DeviceManufacturer,
                DeviceModel = input.DeviceModel,
                DevicePlatform = input.DevicePlatform, 
                DeviceType = input.DeviceType
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
                    ProfilePictureSource = "/Images/Default/pfp.png"
                };
                user.HashedPassword = StringHasher.HashString(input.Password);

                try
                {
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    return BadRequest(e.Message);
                }

                UserLoginInput loginInput = new UserLoginInput
                {
                    Username = input.Username,
                    Password = input.Password
                };
                return await LoginUser(loginInput);
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
