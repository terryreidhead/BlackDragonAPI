using System.ComponentModel.DataAnnotations;

namespace BlackDragonAPI.Models
{
    /// <summary>
    /// Represents a request to create or update a user profile.
    /// All properties are optional to support partial updates.
    /// </summary>
    public class UserProfileRequest
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(50)]
        public string? DisplayName { get; set; }

        [Phone]
        [MaxLength(25)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        public string? AddressLine1 { get; set; }

        [MaxLength(100)]
        public string? AddressLine2 { get; set; }

        [MaxLength(60)]
        public string? City { get; set; }

        [MaxLength(25)]
        public string? State { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(20)]
        public string? BeltLevel { get; set; }
    }
}
