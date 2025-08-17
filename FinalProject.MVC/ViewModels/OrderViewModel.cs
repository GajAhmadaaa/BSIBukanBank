using System.ComponentModel.DataAnnotations;

namespace FinalProject.MVC.ViewModels
{
    public class SimplifiedOrderViewModel
    {
        [Required(ErrorMessage = "Please select a dealer.")]
        [Display(Name = "Dealer")]
        public int DealerId { get; set; }

        [Required(ErrorMessage = "Please select a car.")]
        [Display(Name = "Car Model")]
        public int CarId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Quantity must be between 1 and 5.")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Order Note (Optional)")]
        public string? Note { get; set; }
    }
}