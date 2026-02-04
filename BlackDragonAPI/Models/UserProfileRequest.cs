using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace BlackDragonAPI.Models
{
 /// <summary>
 /// Represents a request to create or update a user profile, including personal and contact information.
 /// </summary>
 /// <remarks>All properties are optional, allowing partial updates to user profiles. This class is typically
 /// used in user management operations where only the fields to be changed need to be provided.</remarks>
    public class UserProfileRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Username { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public string? Address { get; set; }
        public string BeltLevel { get; set; } = "White";
    }

}
