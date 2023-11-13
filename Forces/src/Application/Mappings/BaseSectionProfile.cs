using AutoMapper;
using Forces.Application.Features.BaseSections.Commands.AddEdit;
using Forces.Application.Features.BaseSections.Queries.GetAll;
using Forces.Application.Features.BaseSections.Queries.GetByBaseId;
using Forces.Application.Features.BaseSections.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Mappings
{
    public class BaseSectionProfile : Profile
    {
        public BaseSectionProfile()
        {
            CreateMap<AddEditBaseSectionCommand, Models.BasesSections>().ReverseMap();
            CreateMap<GetAllBasesSectionsQueryResponse, Models.BasesSections>().ReverseMap();
            CreateMap<GetAllSectionsByBaseIdQueryResponse, Models.BasesSections>().ReverseMap();
            CreateMap<GetBaseSectionByIdQueryResponse, Models.BasesSections>().ReverseMap();
        }
    }
}
