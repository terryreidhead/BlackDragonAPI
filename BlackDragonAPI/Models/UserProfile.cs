using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlackDragonAPI.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(450)] // matches typical AspNetUsers.Id length when using string keys
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public IdentityUser? IdentityUser { get; set; }

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

        [Required]
        [MaxLength(20)]
        public string BeltLevel { get; set; } = "White";

        [Required]
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;
    }
}
