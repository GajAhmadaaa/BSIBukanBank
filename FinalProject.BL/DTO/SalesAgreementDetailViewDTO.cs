using System;

namespace FinalProject.BL.DTO;

public class SalesAgreementDetailViewDTO
{
    public int SalesAgreementDetailId { get; set; }
    public int CarId { get; set; }
    public string CarName { get; set; } = string.Empty;
    public decimal AgreedPrice { get; set; }
    public decimal? Discount { get; set; }
    public string? Note { get; set; }
}