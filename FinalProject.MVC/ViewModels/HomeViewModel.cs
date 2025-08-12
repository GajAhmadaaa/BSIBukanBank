using FinalProject.MVC.Models;

namespace FinalProject.MVC.ViewModels
{
    public class HomeViewModel
    {
        public List<CarModel> FeaturedCars { get; set; } = new List<CarModel>();
        public List<TestimonialModel> Testimonials { get; set; } = new List<TestimonialModel>();
    }
}