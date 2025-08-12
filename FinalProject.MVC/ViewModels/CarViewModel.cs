using FinalProject.BO.Models;

namespace FinalProject.MVC.ViewModels
{
    public class CarViewModel
    {
        public int CarId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string CarType { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int? Year { get; set; }
        public string Color { get; set; } = string.Empty;
    }
}