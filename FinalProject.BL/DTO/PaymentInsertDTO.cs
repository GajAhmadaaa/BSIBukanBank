using System;

namespace FinalProject.BL.DTO
{
    public class PaymentInsertDTO
    {
        public int SalesAgreementID { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
    }
}
