using AutoMapper;
using Forces.Application.Features.AirCraft.Commands.AddEdit;
using Forces.Application.Features.AirCraft.Queries.GetAll;
using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Mappings
{
    public class AirCraftProfile : Profile
    {
        public AirCraftProfile()
        {
            CreateMap<AddEditAirCraftCommand, AirCraft>().ReverseMap();
            CreateMap<AirCraft,GetAllAirCraftResponse>().ForMember(x=>x.Base,x=>x.MapFrom(z=>z.Bases.BaseCode))
                .ForMember(x => x.AirKind, x => x.MapFrom(z => z.AirKind.AirKindName))
                .ForMember(x => x.BaseSection, x => x.MapFrom(z => z.BaseSection.SectionCode));
        }
    }
}
