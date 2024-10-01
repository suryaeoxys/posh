using AutoMapper;
using Posh_TRPT_Domain.Register;
using Posh_TRPT_Models.DTO.RegisterDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Services.Mapping
{
    public class VehicleDetailMapProfile:Profile
    {
        public VehicleDetailMapProfile() {

            CreateMap<VehicleDetail, VehicleDetailDTO>()
                .ForMember(dest => dest.Id,
                            opt => opt.MapFrom(src => $"{src.Id}")
                           )
                .ForMember(dest => dest.UserId,
                            opt => opt.MapFrom(src => $"{src.UserId}")
                  )
                .ForMember(dest => dest.CreatedDate,
                            opt => opt.MapFrom(src => $"{src.CreatedDate}")
                           )
                .ForMember(dest => dest.UpdatedDate,
                            opt => opt.MapFrom(src => $"{src.UpdatedDate}")
                  )
                .ForMember(dest => dest.IsActive,
                            opt => opt.MapFrom(src => $"{src.IsActive}")
                           )
                .ForMember(dest => dest.IsDeleted,
                            opt => opt.MapFrom(src => $"{src.IsDeleted}")
                  )
                 .ForMember(dest => dest.UpdatedBy,
                            opt => opt.MapFrom(src => $"{src.UpdatedBy}")
                           )
                .ForMember(dest => dest.Vehicle_Identification_Number,
                            opt => opt.MapFrom(src => $"{src.Vehicle_Identification_Number}")
                  )
                 .ForMember(dest => dest.Make,
                            opt => opt.MapFrom(src => $"{src.Make}")
                           )
                .ForMember(dest => dest.Model,
                            opt => opt.MapFrom(src => $"{src.Model}")
                  )
                .ForMember(dest => dest.Color,
                            opt => opt.MapFrom(src => $"{src.Color}")
                  )
                 .ForMember(dest => dest.Year,
                            opt => opt.MapFrom(src => $"{src.Year}")
                           )
                .ForMember(dest => dest.Vehicle_Plate,
                            opt => opt.MapFrom(src => $"{src.Vehicle_Plate}")
                  )
                
                .ReverseMap();
        }
    }
}
