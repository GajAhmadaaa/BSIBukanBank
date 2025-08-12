using System;
using System.Collections.Generic;

namespace FinalProject.BL.DTO;

public class LetterOfIntentWithDetailsInsertDTO
{
    // LetterOfIntent properties
    public int DealerId { get; set; }
    public int CustomerId { get; set; }
    public int SalesPersonId { get; set; }
    public int? ConsultHistoryId { get; set; }
    public int? TestDriveId { get; set; }
    public DateTime Loidate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Note { get; set; }
    
    // LetterOfIntentDetails collection
    public IEnumerable<LetterOfIntentDetailInsertDTO> Details { get; set; } = new List<LetterOfIntentDetailInsertDTO>();
}