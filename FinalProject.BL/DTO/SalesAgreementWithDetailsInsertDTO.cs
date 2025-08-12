using System;
using System.Collections.Generic;

namespace FinalProject.BL.DTO;

public class SalesAgreementWithDetailsInsertDTO
{
    // SalesAgreement properties
    public int DealerId { get; set; }
    public int CustomerId { get; set; }
    public int SalesPersonId { get; set; }
    public int? Loiid { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? Status { get; set; }
    
    // SalesAgreementDetails collection
    public IEnumerable<SalesAgreementDetailInsertDTO> Details { get; set; } = new List<SalesAgreementDetailInsertDTO>();
}