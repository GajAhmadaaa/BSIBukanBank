using System;
using System.Collections.Generic;

namespace FinalProject.BL.DTO;

public class LetterOfIntentViewDTO
{
    public int Id { get; set; }
    public int DealerId { get; set; }
    public int CustomerId { get; set; }
    public int SalesPersonId { get; set; }
    public int? ConsultHistoryId { get; set; }
    public int? TestDriveId { get; set; }
    public DateTime Loidate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Note { get; set; }
    public string Status { get; set; } = string.Empty;
    
    public IEnumerable<LetterOfIntentDetailViewDTO> Details { get; set; } = new List<LetterOfIntentDetailViewDTO>();
}