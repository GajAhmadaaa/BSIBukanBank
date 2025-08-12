using System.ComponentModel.DataAnnotations;

namespace FinalProject.MVC.ViewModels
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Nama")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Nomor Telepon")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Alamat")]
        public string Address { get; set; } = string.Empty;
    }
}