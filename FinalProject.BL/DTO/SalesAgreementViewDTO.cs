using System;

namespace FinalProject.BL.DTO;

public class SalesAgreementViewDTO
{
    public int Id { get; set; }
    public int DealerId { get; set; }
    public int CustomerId { get; set; }
    public int SalesPersonId { get; set; }
    public int? Loiid { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? Status { get; set; }
}