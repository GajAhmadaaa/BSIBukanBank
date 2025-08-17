using System;

namespace FinalProject.BL.DTO
{
    public class CustomerNotificationUpdateDTO
    {
        public int CustomerNotificationId { get; set; }
        public int CustomerId { get; set; }
        public int? Loid { get; set; }
        public int? SalesAgreementId { get; set; }
        public string NotificationType { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public bool IsRead { get; set; }
    }
}