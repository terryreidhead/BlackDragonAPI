using System.ComponentModel.DataAnnotations;

namespace BlackDragonAPI.Models
{
    public class RegisterUserRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(32,ErrorMessage = "Password must be between 12 and 32 characters long!🥋", MinimumLength = 12)]
        public string Password { get; set; }

        public string Username { get; set; }

        public string BeltLevel { get; set; } = "White"; 

        public bool IsSubscribed { get; set; } = false;

        public bool IsInPersonStudent { get; set; } = false;
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }

}
