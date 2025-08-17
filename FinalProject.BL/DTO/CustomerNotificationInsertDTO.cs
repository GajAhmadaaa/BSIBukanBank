using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.BL.DTO
{
    public class CustomerNotificationInsertDTO
    {
        public int CustomerId { get; set; }
        public int? Loid { get; set; }
        public int? SalesAgreementId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string NotificationType { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Message { get; set; }
    }
}