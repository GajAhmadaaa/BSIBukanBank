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
            
            // Mapping untuk CustomerNotification
            CreateMap<CustomerNotification, CustomerNotificationViewDTO>().ReverseMap();
            CreateMap<CustomerNotification, CustomerNotificationInsertDTO>().ReverseMap();
            CreateMap<CustomerNotification, CustomerNotificationUpdateDTO>().ReverseMap();
            
            // Mapping untuk Dealer
            CreateMap<Dealer, DealerViewDTO>().ReverseMap();
            CreateMap<Dealer, DealerInsertDTO>().ReverseMap();
            CreateMap<Dealer, DealerUpdateDTO>().ReverseMap();
            
            // Mapping untuk DealerInventory
            CreateMap<DealerInventory, DealerInventoryViewDTO>().ReverseMap();
            CreateMap<DealerInventory, DealerInventoryInsertDTO>().ReverseMap();
            CreateMap<DealerInventory, DealerInventoryUpdateDTO>().ReverseMap();
            
            // Mapping untuk LetterOfIntent
            CreateMap<LetterOfIntent, LetterOfIntentViewDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Loiid))
                .ReverseMap()
                .ForMember(dest => dest.Loiid, opt => opt.MapFrom(src => src.Id));
            CreateMap<LetterOfIntent, LetterOfIntentInsertDTO>().ReverseMap();
            CreateMap<LetterOfIntent, LetterOfIntentUpdateDTO>().ReverseMap();
            CreateMap<LetterOfIntentWithDetailsInsertDTO, LetterOfIntent>()
                .ForMember(dest => dest.LetterOfIntentDetails, opt => opt.Ignore());
            
            // Mapping untuk LetterOfIntentDetail
            CreateMap<LetterOfIntentDetail, LetterOfIntentDetailViewDTO>()
                .ForMember(dest => dest.AgreedPrice, opt => opt.MapFrom(src => src.AgreedPrice))
                .ForMember(dest => dest.CarName, opt => opt.MapFrom(src => src.Car.Model));
            CreateMap<LetterOfIntentDetailInsertDTO, LetterOfIntentDetail>()
                .ForMember(dest => dest.AgreedPrice, opt => opt.MapFrom(src => src.Price));
            
            // Mapping untuk SalesAgreement
            CreateMap<SalesAgreement, SalesAgreementViewDTO>().ReverseMap();
            CreateMap<SalesAgreement, SalesAgreementInsertDTO>().ReverseMap();
            CreateMap<SalesAgreement, SalesAgreementUpdateDTO>().ReverseMap();
            CreateMap<SalesAgreementWithDetailsInsertDTO, SalesAgreement>()
                .ForMember(dest => dest.SalesAgreementDetails, opt => opt.Ignore());
            
            // Mapping untuk SalesAgreementDetail
            CreateMap<SalesAgreementDetail, SalesAgreementDetailViewDTO>()
                .ForMember(dest => dest.AgreedPrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CarName, opt => opt.MapFrom(src => src.Car.Model));
            CreateMap<SalesAgreementDetailInsertDTO, SalesAgreementDetail>();
            
            // Mapping untuk SalesPerson
            CreateMap<SalesPerson, SalesPersonViewDTO>().ReverseMap();
            CreateMap<SalesPerson, SalesPersonInsertDTO>().ReverseMap();
            CreateMap<SalesPerson, SalesPersonUpdateDTO>().ReverseMap();
        }
    }
}