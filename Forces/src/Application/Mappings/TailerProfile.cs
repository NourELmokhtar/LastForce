using AutoMapper;
using Forces.Application.Features.Tailers.Commands.AddEdit;
using Forces.Application.Features.Tailers.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Mappings
{
    public class TailerProfile : Profile
    {
        public TailerProfile()
        {
            CreateMap<AddEditTailerCommand, Models.Tailers>().ReverseMap();
            CreateMap<TailerDto, Models.Tailers>().ReverseMap();
        }
    }
}
