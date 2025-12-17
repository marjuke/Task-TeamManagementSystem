using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string? DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public required string Password { get; set; }
       


    }
}
