using BlackDragonAPI.Data;
using BlackDragonAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlackDragonAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(ApplicationDbContext context, UserManager<IdentityUser> userManager, ILogger<ProfileController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                email = user.Email ?? "Email not set",
                userName = user.UserName ?? "Username not set",
                id = user.Id
            });
        }

        [HttpPost("profile")]
        public async Task<IActionResult> CreateProfile([FromBody] UserProfileRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var profile = new UserProfile
            {
                UserId = user.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Username = request.Username,
                Gender = request.Gender,
                Age = request.Age,
                Address = request.Address,
                BeltLevel = request.BeltLevel
            };

            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return Ok(profile);
        }
    }
}
