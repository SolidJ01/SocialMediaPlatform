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
                token.LastAccessed = DateTime.Now;
                try
                {
                    _context.Update(user);
                    _context.Update(token);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    return Problem(e.Message);
                }

            }

            return BadRequest();
        }

        [HttpPost]
        [Route("/Follow")]
        public async Task<ActionResult> FollowUser(UserFollowRequest request)
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

                User userToFollow = _context.Users.Where(x => x.Id.Equals(request.FollowId)).Include(x => x.Followers).First();

                if (userToFollow != null)
                {
                    userToFollow.Followers.Add(user);
                }

                token.LastAccessed = DateTime.Now;

                try
                {
                    _context.Update(userToFollow);
                    _context.Update(token);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    return Problem(e.Message);
                }

            }

            return BadRequest();
        }

        [HttpPost]
        [Route("/CreatePost")]
        public async Task<ActionResult> CreatePost(UserPostCreationRequest request)
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

                Post post = new Post
                {
                    Title = request.Title,
                    Body = request.Body,
                    Poster = user,
                    Posted = DateTime.Now
                };

                token.LastAccessed = DateTime.Now;

                try
                {
                    _context.Posts.Add(post);
                    _context.Update(token);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    return Problem(e.Message);
                }

            }

            return BadRequest();
        }

        [HttpPost]
        [Route("/Feed")]
        public async Task<ActionResult<List<Post>>> GetFeed(UserAuthorizationInput request)
        {
            //  TODO: Needs to be expanded for "pages" for infinite scroll refresh
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
                user.Following = _context.Users.Find(user.Id).Following;
                if (user.Following == null)
                {
                    user.Following = new List<User>();
                }

                List<Post> posts = await _context.Posts.Where(x => x.PosterId.Equals(user.Id) || user.Following.Contains(x.Poster)).OrderByDescending(x => x.Posted).ToListAsync();

                //  Avoid object loops in the JSON
                foreach (Post post in posts)
                {
                    post.Poster = null;
                    post.LikedBy = null;
                }
                return Ok(posts);
            }

            return BadRequest();

        }

        private bool TokenExists(string token)
        {
            return _context.LoginTokens.Any(e => e.HashedValue.Equals(StringHasher.HashString(token)));
        }
    }
}
