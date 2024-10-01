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
    public class CityResponseMapProfile:Profile
    {
        public CityResponseMapProfile()
        {
           

            CreateMap<CityDTO, City>().ForMember(
                  dest => dest.Id,
                  opt => opt.MapFrom(src => $"{src.Id}")
              )
              .ForMember(
                  dest => dest.Name,
                  opt => opt.MapFrom(src => $"{src.Name}")
              )
            .ReverseMap();

            CreateMap<StateDTO, State>()
 .ReverseMap();

        }
    }
}
