using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace FinalProject.BO.Models
{
    public partial class CustomerNotification
    {
        [Key]
        [Column("CustomerNotificationID")]
        public int CustomerNotificationId { get; set; }

        [Column("CustomerID")]
        public int CustomerId { get; set; }

        [Column("LOIID", TypeName = "int")]
        public int? Loiid { get; set; } // Nullable, opsional

        [Column("SalesAgreementID", TypeName = "int")]
        public int? SalesAgreementId { get; set; } // Nullable, opsional

        [Required]
        [Column("NotificationType")]
        [StringLength(50)]
        public string NotificationType { get; set; }

        [Required]
        [Column("Message")]
        [StringLength(500)]
        public string Message { get; set; }

        [Column("CreatedDate", TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [Column("ReadDate", TypeName = "datetime")]
        public DateTime? ReadDate { get; set; }

        [Column("IsRead")]
        public bool IsRead { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("CustomerNotifications")]
        public virtual Customer Customer { get; set; }

        [ForeignKey(nameof(Loiid))]
        [InverseProperty(nameof(BO.Models.LetterOfIntent.CustomerNotifications))]
        public virtual LetterOfIntent LetterOfIntent { get; set; }

        [ForeignKey(nameof(SalesAgreementId))]
        [InverseProperty(nameof(BO.Models.SalesAgreement.CustomerNotifications))]
        public virtual SalesAgreement SalesAgreement { get; set; }
    }
}