using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BlackDragonAPI.Models
{
    /// <summary>
    /// Represents a user profile that contains personal and contact information associated with an application user.
    /// </summary>
    /// <remarks>The UserProfile class stores user-specific data such as names, contact details, and
    /// demographic information. It is linked to the application's user account through the UserId property, which
    /// references an IdentityUser. This class is typically used to extend the identity system with additional profile
    /// information required by the application.</remarks>
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser IdentityUser { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Username { get; set; }

        public string? Gender { get; set; }

        public int? Age { get; set; }

        public string? Address { get; set; }

        public string BeltLevel { get; set; }
    }
}
