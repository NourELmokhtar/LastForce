using AutoMapper;
using Forces.Application.Features.AirKind.Commands.AddEdit;
using Forces.Application.Features.AirKind.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Mappings
{
    public class AirKindProfile : Profile
    {
        public AirKindProfile()
        {
            CreateMap<AddEditAirKindCommand, Models.AirKind>().ReverseMap();
            CreateMap<Models.AirKind, GetAllAirKindResponse>().ForMember(x => x.AirTypeName, x => x.MapFrom(z => z.AirType.AirTypeName));
        }
    }
}
