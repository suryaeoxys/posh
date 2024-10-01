using AutoMapper;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Models.DTO.MasterTableDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.Mapping
{
    public class StateResponseMapProfile:Profile
    {
        public StateResponseMapProfile()
        {
          


            CreateMap<StateDTO, State>()
             .ForMember(
                 dest => dest.Id,
                 opt => opt.MapFrom(src => $"{src.Id}")
             )
             .ForMember(
                 dest => dest.Name,
                 opt => opt.MapFrom(src => $"{src.Name}")
             ).ReverseMap();


            CreateMap<CountryDTO, Country>().ReverseMap();
        }
     
    }
}
