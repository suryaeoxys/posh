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
    public class TokenInfoUserMapProfile:Profile
    {
        public TokenInfoUserMapProfile()
        {
            CreateMap<TokenInfoUserDTO, TokenInfoUser>()
                   .ForMember(
                    dest => dest.UserName,
                    opt => opt.MapFrom(src => $"{src.UserName}")
                )
                 .ForMember(
                    dest => dest.RefreshToken,
                    opt => opt.MapFrom(src => $"{src.RefreshToken}")
                )
                  .ForMember(
                    dest => dest.RefreshTokenExpiry,
                    opt => opt.MapFrom(src => $"{src.RefreshTokenExpiry}")
                )
                   .ForMember(
                    dest => dest.IsDeleted,
                    opt => opt.MapFrom(src => $"{src.IsDeleted}")
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
                ).ReverseMap();
        }
    }
}
