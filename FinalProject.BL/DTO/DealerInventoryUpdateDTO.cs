using System;

namespace FinalProject.BL.DTO;

public class DealerInventoryUpdateDTO
{
    public int Id { get; set; }
    public int DealerId { get; set; }
    public int CarId { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public double DiscountPercent { get; set; }
    public double FeePercent { get; set; }
}