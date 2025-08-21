using FinalProject.BL.DTO;
using System.Collections.Generic;

namespace FinalProject.MVC.ViewModels
{
    public class OrderIndexViewModel
    {
        public List<LetterOfIntentViewDTO> PendingLois { get; set; } = new List<LetterOfIntentViewDTO>();
        public List<SalesAgreementViewDTO> UnpaidAgreements { get; set; } = new List<SalesAgreementViewDTO>();
        public List<SalesAgreementViewDTO> PaidAgreements { get; set; } = new List<SalesAgreementViewDTO>();
    }
}