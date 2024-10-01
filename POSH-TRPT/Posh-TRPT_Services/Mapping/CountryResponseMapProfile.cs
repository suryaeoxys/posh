using AutoMapper;
using Posh_TRPT_Domain.Employees;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Models.DTO;
using Posh_TRPT_Models.DTO.MasterTableDTO;
using Posh_TRPT_Models.DTO.RegisterDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.Mapping
{
    public class CountryResponseMapProfile:Profile
    {
        public CountryResponseMapProfile()
        {

            CreateMap<CountryData, Country>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => $"{src.Id}")
            )
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => $"{src.Name}")
            ).ReverseMap();

            CreateMap<CountryDTO, Country>()
             .ForMember(
                 dest => dest.Id,
                 opt => opt.MapFrom(src => $"{src.Id}")
             )
             .ForMember(
                 dest => dest.Name,
                 opt => opt.MapFrom(src => $"{src.Name}")
             )

              .ReverseMap();
        }
    }
}
