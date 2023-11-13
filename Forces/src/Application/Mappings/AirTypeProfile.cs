using AutoMapper;
using Forces.Application.Features.AirType.Commands.AddEdit;
using Forces.Application.Features.AirType.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Mappings
{
    public class AirTypeProfile : Profile
    {
        public AirTypeProfile()
        {
            CreateMap<AddEditAirTypeCommand, Models.AirType>().ReverseMap();
            CreateMap<GetAllAirTypeResponse, Models.AirType>().ReverseMap();
        }
    }
}
