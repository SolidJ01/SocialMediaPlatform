using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaApi.Data;
using SocialMediaApi.Models.Helpers;
using SocialMediaApi.Models.Input;

namespace SocialMediaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUserProfile(UserProfileUpdateRequest request)
        {
            if (!TokenExists(request.LoginToken))
            {
                return NotFound();
            }

            LoginToken token = await _context.LoginTokens.Where(x => x.HashedValue.Equals(StringHasher.HashString(request.LoginToken))).Include(x => x.Owner).FirstAsync();

            if (
                token.DevicePlatform.Equals(request.DevicePlatform) &&
                token.DeviceIdiom.Equals(request.DeviceIdiom) &&
                token.DeviceType.Equals(request.DeviceType) &&
                token.DeviceModel.Equals(request.DeviceModel) &&
                token.DeviceManufacturer.Equals(request.DeviceManufacturer) &&
                (DateTime.Now - token.LastAccessed).TotalDays < 60)
            {
                User user = token.Owner;
                if (!user.Username.Equals(request.Username))
                {
                    user.Username = request.Username;
                }
                if (!user.Email.Equals(request.Email))
                {
                    user.Email = request.Email;
                }
                if (!user.ProfilePictureSource.Equals(request.ProfilePictureSource))
                {
                    user.ProfilePictureSource = request.ProfilePictureSource;
                }
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    return Problem(e.Message);
                }
                return Ok();

            }

            return BadRequest();
        }

        private bool TokenExists(string token)
        {
            return _context.LoginTokens.Any(e => e.HashedValue.Equals(StringHasher.HashString(token)));
        }
    }
}
