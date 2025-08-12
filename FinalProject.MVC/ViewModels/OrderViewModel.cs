using System.ComponentModel.DataAnnotations;

namespace FinalProject.MVC.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Mobil")]
        public int CarId { get; set; }

        [Required]
        [Display(Name = "Tanggal Order")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Display(Name = "Catatan")]
        public string Note { get; set; } = string.Empty;

        // Informasi mobil
        public string CarModel { get; set; } = string.Empty;
        public string CarBrand { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // Informasi customer dari user yang login
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }
}