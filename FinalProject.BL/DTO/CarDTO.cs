namespace FinalProject.BL.DTO
{
    public class CarDTO
    {
        public int CarId { get; set; }
        public string Model { get; set; }
        public string CarType { get; set; }
        public decimal BasePrice { get; set; }
        public int? Year { get; set; }
        public string Color { get; set; }
    }
}