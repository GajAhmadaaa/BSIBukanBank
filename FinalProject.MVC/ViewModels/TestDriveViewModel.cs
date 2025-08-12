using FinalProject.BO.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.MVC.ViewModels
{
    public class TestDriveViewModel
    {
        public int TestDriveId { get; set; }

        [Required]
        [Display(Name = "Tanggal Test Drive")]
        public DateTime TestDriveDate { get; set; }

        [Display(Name = "Catatan")]
        public string Note { get; set; } = string.Empty;

        // Foreign keys
        public int DealerId { get; set; }
        public int CustomerId { get; set; }
        public int SalesPersonId { get; set; }
        public int CarId { get; set; }
        public int? ConsultHistoryId { get; set; }

        // Navigation properties for display
        public string DealerName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string SalesPersonName { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
    }
}