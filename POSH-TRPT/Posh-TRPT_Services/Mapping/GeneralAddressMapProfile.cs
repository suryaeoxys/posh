using AutoMapper;
using OtpNet;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Models.DTO.RegisterDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.Mapping
{
    public class GeneralAddressMapProfile:Profile
    {
        public GeneralAddressMapProfile()
        {
            CreateMap<GeneralAddress, GeneralAddressDTO>()
            .ForMember(dest => dest.Id,
                        opt => opt.MapFrom(src => $"{src.Id}")
                       )
            .ForMember(dest => dest.UserId,
                        opt => opt.MapFrom(src => $"{src.UserId}")
              )
            .ForMember(dest => dest.City,
                        opt => opt.MapFrom(src => $"{src.City}")
                       )
            .ForMember(dest => dest.Country,
                        opt => opt.MapFrom(src => $"{src.Country}")
              )
            .ForMember(dest => dest.State,
                        opt => opt.MapFrom(src => $"{src.State}")
                       )
            .ForMember(dest => dest.Social_Security_Number,
                        opt => opt.MapFrom(src => $"{src.Social_Security_Number}")
              )
             .ForMember(dest => dest.IsDeleted,
                        opt => opt.MapFrom(src => $"{src.IsDeleted}")
                       )
            .ForMember(dest => dest.CreatedDate,
                        opt => opt.MapFrom(src => $"{src.CreatedDate}")
              )
             .ForMember(dest => dest.UpdatedDate,
                        opt => opt.MapFrom(src => $"{src.UpdatedDate}")
                       )
            .ForMember(dest => dest.UpdatedBy,
                        opt => opt.MapFrom(src => $"{src.UpdatedBy}")
              ).ReverseMap();
        }
        
    }
}
