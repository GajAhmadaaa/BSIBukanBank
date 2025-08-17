using System.ComponentModel.DataAnnotations;

namespace FinalProject.BL.DTO
{
    public class CustomerRegistrationDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public CustomerInsertDTO CustomerData { get; set; }
    }
}