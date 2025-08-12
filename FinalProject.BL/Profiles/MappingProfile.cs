using AutoMapper;
using FinalProject.BL.DTO;
using FinalProject.BO.Models;

namespace FinalProject.BL.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping untuk Car
            CreateMap<Car, CarViewDTO>().ReverseMap();
            CreateMap<Car, CarInsertDTO>().ReverseMap();
            CreateMap<Car, CarUpdateDTO>().ReverseMap();
            
            // Mapping untuk Customer
            CreateMap<Customer, CustomerViewDTO>().ReverseMap();
            CreateMap<Customer, CustomerInsertDTO>().ReverseMap();
            CreateMap<Customer, CustomerUpdateDTO>().ReverseMap();
            
            // Mapping untuk Dealer
            CreateMap<Dealer, DealerViewDTO>().ReverseMap();
            CreateMap<Dealer, DealerInsertDTO>().ReverseMap();
            CreateMap<Dealer, DealerUpdateDTO>().ReverseMap();
            
            // Mapping untuk DealerInventory
            CreateMap<DealerInventory, DealerInventoryViewDTO>().ReverseMap();
            CreateMap<DealerInventory, DealerInventoryInsertDTO>().ReverseMap();
            CreateMap<DealerInventory, DealerInventoryUpdateDTO>().ReverseMap();
            
            // Mapping untuk LetterOfIntent
            CreateMap<LetterOfIntent, LetterOfIntentViewDTO>().ReverseMap();
            CreateMap<LetterOfIntent, LetterOfIntentInsertDTO>().ReverseMap();
            CreateMap<LetterOfIntent, LetterOfIntentUpdateDTO>().ReverseMap();
            CreateMap<LetterOfIntentWithDetailsInsertDTO, LetterOfIntent>()
                .ForMember(dest => dest.LetterOfIntentDetails, opt => opt.Ignore());
            
            // Mapping untuk LetterOfIntentDetail
            CreateMap<LetterOfIntentDetailInsertDTO, LetterOfIntentDetail>();
            
            // Mapping untuk SalesAgreement
            CreateMap<SalesAgreement, SalesAgreementViewDTO>().ReverseMap();
            CreateMap<SalesAgreement, SalesAgreementInsertDTO>().ReverseMap();
            CreateMap<SalesAgreement, SalesAgreementUpdateDTO>().ReverseMap();
            CreateMap<SalesAgreementWithDetailsInsertDTO, SalesAgreement>()
                .ForMember(dest => dest.SalesAgreementDetails, opt => opt.Ignore());
            
            // Mapping untuk SalesAgreementDetail
            CreateMap<SalesAgreementDetailInsertDTO, SalesAgreementDetail>();
            
            // Mapping untuk SalesPerson
            CreateMap<SalesPerson, SalesPersonViewDTO>().ReverseMap();
            CreateMap<SalesPerson, SalesPersonInsertDTO>().ReverseMap();
            CreateMap<SalesPerson, SalesPersonUpdateDTO>().ReverseMap();
        }
    }
}