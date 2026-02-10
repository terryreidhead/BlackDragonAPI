using BlackDragonAPI.Data;
using BlackDragonAPI.Models;
using BlackDragonAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlackDragonAPI.Controllers
{
    /// <summary>
    /// Handles operations related to the authenticated user's profile, including retrieving and updating profile
    /// information.
    /// </summary>
    /// <remarks>This controller requires authorization for all endpoints. Use the 'GetMe' endpoint to
    /// retrieve the current user's profile, and the 'UpsertMe' endpoint to create or update profile details. All
    /// profile data is managed through the application's database context. Endpoints are accessible only to
    /// authenticated users.</remarks>
    [ApiController]
    [Route("api/profile")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserContext _userContext;

        public ProfileController(ApplicationDbContext context, IUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserProfileResponse>> GetMe()
        {
            var userId = _userContext.UserId;
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized(new { message = "Missing user identity." });

                 var profile = await _context.UserProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
                return NotFound();

            return Ok(ToResponse(profile));
        }

        /// <summary>
        /// Updates the profile of the currently authenticated user or creates a new profile if none exists.
        /// </summary>
        /// <remarks>This method requires the user to be authenticated. If the user is not authenticated,
        /// it returns an Unauthorized result. The method applies any changes from the provided request to the existing
        /// profile or creates a new profile if one does not exist.</remarks>
        /// <param name="request">The user profile data to be updated or created. Must include valid profile information such as belt level
        /// and other relevant fields.</param>
        /// <returns>An ActionResult containing a UserProfileResponse that represents the updated or newly created user profile.</returns>
        [HttpPut("me")]
        public async Task<ActionResult<UserProfileResponse>> UpsertMe([FromBody] UserProfileRequest request)
        {
            var userId = _userContext.UserId;
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized(new { message = "Missing user identity." });

            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
            {
                profile = new UserProfile
                {
                    UserId = userId,
                    BeltLevel = "White",
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow
                };

                ApplyPatch(profile, request);

                _context.UserProfiles.Add(profile);
                await _context.SaveChangesAsync();

                return Ok(ToResponse(profile));
            }

            ApplyPatch(profile, request);
            profile.UpdatedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(ToResponse(profile));
        }

        /// <summary>
        /// Applies non-null values from the user profile request to the specified user profile.
        /// </summary>
        /// <remarks>This method updates only those fields in the user profile for which the corresponding
        /// values in the request are not null. Ensure that the request contains valid and intended data before calling
        /// this method.</remarks>
        /// <param name="profile">The user profile to be updated with values from the request.</param>
        /// <param name="request">The request containing updated user profile information. Only fields with non-null values will be applied.</param>
        private static void ApplyPatch(UserProfile profile, UserProfileRequest request)
        {
            if (request.FirstName != null) profile.FirstName = request.FirstName;
            if (request.LastName != null) profile.LastName = request.LastName;
            if (request.DisplayName != null) profile.DisplayName = request.DisplayName;
            if (request.PhoneNumber != null) profile.PhoneNumber = request.PhoneNumber;

            if (request.AddressLine1 != null) profile.AddressLine1 = request.AddressLine1;
            if (request.AddressLine2 != null) profile.AddressLine2 = request.AddressLine2;
            if (request.City != null) profile.City = request.City;
            if (request.State != null) profile.State = request.State;
            if (request.PostalCode != null) profile.PostalCode = request.PostalCode;

            if (request.BeltLevel != null) profile.BeltLevel = request.BeltLevel;
        }

        /// <summary>
        /// Creates a new UserProfileResponse instance containing user information mapped from the specified
        /// UserProfile.
        /// </summary>
        /// <remarks>Use this method to transform internal user profile data into a format suitable for
        /// API responses. All relevant fields are copied directly from the source profile.</remarks>
        /// <param name="profile">The UserProfile object that provides the source user data to be converted. Cannot be null.</param>
        /// <returns>A UserProfileResponse object populated with the corresponding user details from the provided UserProfile.</returns>
        private static UserProfileResponse ToResponse(UserProfile profile)
        {
            return new UserProfileResponse
            {
                Id = profile.Id,
                UserId = profile.UserId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                DisplayName = profile.DisplayName,
                PhoneNumber = profile.PhoneNumber,
                AddressLine1 = profile.AddressLine1,
                AddressLine2 = profile.AddressLine2,
                City = profile.City,
                State = profile.State,
                PostalCode = profile.PostalCode,
                BeltLevel = profile.BeltLevel,
                CreatedUtc = profile.CreatedUtc,
                UpdatedUtc = profile.UpdatedUtc
            };
        }
    }
}
