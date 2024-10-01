using AutoMapper;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Models.DTO.RoleMenuDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.Mapping
{
    public class MenuMasterMapProfile : Profile
    {
        public MenuMasterMapProfile()
        {
            CreateMap<RoleMenuDTO, MenuMaster>()
               .ForMember(
                   dest => dest.Id,
                   opt => opt.MapFrom(src => $"{src.MenuIdentity}")
               )
               .ForMember(
                   dest => dest.MenuID,
                   opt => opt.MapFrom(src => $"{src.MenuID}")
               )
               .ForMember(
                   dest => dest.MenuName,
                   opt => opt.MapFrom(src => $"{src.MenuName}")
               )
                .ForMember(
                   dest => dest.Parent_MenuID,
                   opt => opt.MapFrom(src => $"{src.Parent_MenuID}")
               )
               .ForMember(
                   dest => dest.User_Roll,
                   opt => opt.MapFrom(src => $"{src.User_Roll}")
               ).ForMember(
                   dest => dest.USE_YN,
                   opt => opt.MapFrom(src => $"{src.USE_YN}")
               )
                .ForMember(
                   dest => dest.CreatedDate,
                   opt => opt.MapFrom(src => $"{src.CreatedDate}")
               )
                .ForMember(
                   dest => dest.MenuURL,
                   opt => opt.MapFrom(src => $"{src.MenuURL}")
               )
                .ReverseMap();
        }
    }
}
