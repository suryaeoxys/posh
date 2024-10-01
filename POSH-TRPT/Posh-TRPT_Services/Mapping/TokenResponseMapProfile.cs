using AutoMapper;
using Posh_TRPT_Domain.Token;
using Posh_TRPT_Models.DTO.TokenDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.Mapping
{
    public class TokenResponseMapProfile:Profile
    {
        public TokenResponseMapProfile()
        {
            CreateMap<TokenResponseDTO, TokenResponse>()
               .ForMember(
                    dest => dest.TokenString,
                    opt => opt.MapFrom(src => $"{src.TokenString}")
                )
                .ForMember(
                    dest => dest.ValidTo,
                    opt => opt.MapFrom(src => $"{src.ValidTo}")
                ).ReverseMap();
        }
    }
}
