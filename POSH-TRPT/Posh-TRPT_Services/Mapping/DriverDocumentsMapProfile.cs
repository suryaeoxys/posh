using AutoMapper;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Models.DTO.DriverDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.Mapping
{
    public class DriverDocumentsMapProfile:Profile
    {
        public DriverDocumentsMapProfile()
        {
            CreateMap<DriverDocumentDTO, DriverDocuments>()
              .ForMember(
                  dest => dest.Id,
                  opt => opt.MapFrom(src => $"{src.Id}")
              )
              .ForMember(
                  dest => dest.DrivingLicenceDoc,
                  opt => opt.MapFrom(src => $"{src.DrivingLicenceDoc}")
              )
              .ForMember(
                  dest => dest.ProfilePhoto,
                  opt => opt.MapFrom(src => $"{src.ProfilePhoto}")
              )
                .ForMember(
                  dest => dest.VehicleRegistrationDoc,
                  opt => opt.MapFrom(src => $"{src.VehicleRegistrationDoc}")
              )
              .ForMember(
                  dest => dest.IsBackgroundVerificationChecked,
                  opt => opt.MapFrom(src => $"{src.IsBackgroundVerificationChecked}")
              )
                .ForMember(
                  dest => dest.PassportDoc,
                  opt => opt.MapFrom(src => $"{src.PassportDoc}")
              )
               .ForMember(
                  dest => dest.InsuarnceDoc,
                  opt => opt.MapFrom(src => $"{src.InsuarnceDoc}")
              )
              .ForMember(
                  dest => dest.ProfilePhotoName,
                  opt => opt.MapFrom(src => $"{src.ProfilePhotoName}")
              )
                .ForMember(
                  dest => dest.VehicleRegistrationDocName,
                  opt => opt.MapFrom(src => $"{src.VehicleRegistrationDocName}")
              )
              .ForMember(
                  dest => dest.DrivingLicenceDocName,
                  opt => opt.MapFrom(src => $"{src.DrivingLicenceDocName}")
              )
              .ForMember(
                  dest => dest.UserId,
                  opt => opt.MapFrom(src => $"{src.UserId}")
              )
              .ForMember(
                  dest => dest.CreatedBy,
                  opt => opt.MapFrom(src => $"{src.CreatedBy}")
              )
               .ForMember(
                  dest => dest.CreatedDate,
                  opt => opt.MapFrom(src => $"{src.CreatedDate}")
              )
                  .ForMember(
                  dest => dest.UpdatedDate,
                  opt => opt.MapFrom(src => $"{src.UpdatedDate}")
              )
                .ForMember(
                  dest => dest.UpdatedBy,
                  opt => opt.MapFrom(src => $"{src.UpdatedBy}")
              )
                .ForMember(
                  dest => dest.IsDeleted,
                  opt => opt.MapFrom(src => $"{src.IsDeleted}")
              )           
              .ReverseMap();
        }
      
    }
}
