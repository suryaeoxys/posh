using AutoMapper;
using Posh_TRPT_Domain.BookingSystem;
using Posh_TRPT_Domain.Employees;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.Entity.Posh_TRPT_Domain.StripePayment;
using Posh_TRPT_Domain.PushNotification;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Domain.StripePayment;
using Posh_TRPT_Models.DTO;
using Posh_TRPT_Models.DTO.BookingSystemDTO;
using Posh_TRPT_Models.DTO.CustomerDTO;
using Posh_TRPT_Models.DTO.MasterTableDTO;
using Posh_TRPT_Models.DTO.PushNotificationDTO;
using Posh_TRPT_Models.DTO.StripePaymentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DriverDetail = Posh_TRPT_Domain.BookingSystem.DriverDetail;

namespace Posh_TRPT_Services.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EmployeeDTO, Employee>()
                .ForMember(
                    dest => dest.UserName,
                    opt => opt.MapFrom(src => $"{src.UserName}")
                )
                .ForMember(
                    dest => dest.Email,
                    opt => opt.MapFrom(src => $"{src.Email}")
                )
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => $"{src.Id}")
                )
                .ReverseMap();

            CreateMap<RideCategoryDTO, RideCategory>().ReverseMap();
            CreateMap<StatusDTO, Status>().ReverseMap();
            CreateMap<CategoryPriceDTO, CategoryPrice>().ReverseMap();
            CreateMap<GoogleNotificationDTO, GoogleNotification>().ReverseMap();
            CreateMap<NotificationModelDTO, NotificationModel>().ReverseMap();
           CreateMap<CustomerDTO, UserMobileData>().ReverseMap();
         

            CreateMap<SourceDTO, Source>().ReverseMap();
            CreateMap<DistanceCalculateDTO, DistanceCalculate>().ReverseMap();
            CreateMap<AvailableCategoryFareDTO, AvailableCategoryFare>().ReverseMap();

            CreateMap<SourceDTO, Source>().ReverseMap();
            CreateMap<DestinationDTO, Destination>().ReverseMap();
            CreateMap<BookingNotificationRequesrDTO, BookingNotificationRequesr>().ReverseMap();
            CreateMap<RiderDetailDTO, Posh_TRPT_Domain.BookingSystem.RiderDetail>().ReverseMap();
            CreateMap<DriverDetailDTO, DriverDetail>().ReverseMap();
            CreateMap<BookingNotificationRequesrDTO, BookingNotificationRequesr>().ReverseMap();
            CreateMap<DriverBookingData, Posh_TRPT_Domain.Entity.BookingDetail>().ReverseMap(); 
			CreateMap<StripeCustomerIntent, StripeCustomerIntentDTO>().ReverseMap();
			CreateMap<PaymentIntentConfirm, PaymentIntentConfirmDTO>().ReverseMap();
			CreateMap<PaymentIntentCapture, PaymentIntentCaptureDTO>().ReverseMap();
			CreateMap<StripeDriverPaymentTransferDetails, StripeDriverPaymentTransferDetailsDTO>().ReverseMap();
            CreateMap<TipsReviews, TipsAndReviewDTO>().ReverseMap();
            CreateMap<DigitalWalletData, DigitalWallet>().ReverseMap();
            CreateMap<BookingCancellation, BookingCancellationDTO>().ReverseMap();

        }
    }
}
