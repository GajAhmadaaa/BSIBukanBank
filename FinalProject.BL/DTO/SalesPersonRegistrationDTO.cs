using System.ComponentModel.DataAnnotations;

namespace FinalProject.BL.DTO
{
    public class SalesPersonRegistrationDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        
        [Required]
        public SalesPersonInsertDTO SalesPersonData { get; set; }
    }
}