namespace FinalProject.MVC.Models
{
    public class TestimonialModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; } // 1-5
        public string CustomerInitials { get; set; } = string.Empty;
    }
}