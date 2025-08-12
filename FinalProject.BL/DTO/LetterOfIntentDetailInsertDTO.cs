using System;

namespace FinalProject.BL.DTO;

public class LetterOfIntentDetailInsertDTO
{
    public int CarId { get; set; }
    public decimal Price { get; set; }
    public decimal? Discount { get; set; }
    public string? Note { get; set; }
}