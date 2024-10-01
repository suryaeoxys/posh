using AutoMapper;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Models.DTO.ApplicationUserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.Mapping
{
    public class ApplicationUserMapProfile:Profile
    {
        public ApplicationUserMapProfile()
        {
            CreateMap<ApplicationUserDTO, ApplicationUser>()
                .ForMember(
                    dest => dest.UserName,
                    opt => opt.MapFrom(src => $"{src.UserName}")
                )
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => $"{src.Id}")
                )
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.Name}")
                )
                .ForMember(
                    dest => dest.DOB,
                    opt => opt.MapFrom(src => $"{src.DOB}")
                )
                .ForMember(
                    dest => dest.ProfilePhoto,
                    opt => opt.MapFrom(src => $"{src.ProfilePhoto}")
                )
                .ForMember(
                    dest => dest.Platform,
                    opt => opt.MapFrom(src => $"{src.Platform}")
                )
                 .ForMember(
                    dest => dest.IsDeleted,
                    opt => opt.MapFrom(src => $"{src.IsDeleted}")
                )
                 .ForMember(
                    dest => dest.IsActive,
                    opt => opt.MapFrom(src => $"{src.IsActive}")
                )
                 .ForMember(
                    dest => dest.CreatedDate,
                    opt => opt.MapFrom(src => $"{src.CreatedDate}")
                )
                .ForMember(
                    dest => dest.CreatedBy,
                    opt => opt.MapFrom(src => $"{src.CreatedBy}")
                )
                 .ForMember(
                    dest => dest.UpdatedDate,
                    opt => opt.MapFrom(src => $"{src.UpdatedDate}")
                )
                   .ForMember(
                    dest => dest.UpdatedBy,
                    opt => opt.MapFrom(src => $"{src.UpdatedBy}")
                ).ReverseMap();
        }
    }
}
