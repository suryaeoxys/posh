using AutoMapper;
using Posh_TRPT_Domain.Employees;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Models.DTO;
using Posh_TRPT_Models.DTO.RegisterDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.Mapping
{
    public class DriverRegisterMapProfile:Profile
    {
        public DriverRegisterMapProfile()
        {
            CreateMap<DriverRegister,DriverRegisterDTO>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.Name}")
                )
                .ForMember(
                    dest => dest.Email,
                    opt => opt.MapFrom(src => $"{src.Email}")
                )
                .ForMember(
                    dest => dest.Password,
                    opt => opt.MapFrom(src => $"{src.Password}")
                )
                  .ForMember(
                    dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src => $"{src.Mobile}")
                )
                .ForMember(
                    dest => dest.DOB,
                    opt => opt.MapFrom(src => $"{src.DOB}")
                )
                 .ForMember(
                    dest => dest.DeviceId,
                    opt => opt.MapFrom(src => $"{src.DeviceId}")
                )
                .ForMember(
                    dest => dest.Platform,
                    opt => opt.MapFrom(src => $"{src.Platform}")
                )
                .ReverseMap();
        }
    }
}
