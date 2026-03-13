using System.ComponentModel.DataAnnotations;

namespace southernTravel.DTOs
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string Birthday { get; set; } = null!;
    }
}
