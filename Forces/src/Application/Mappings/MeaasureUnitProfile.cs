using AutoMapper;
using Forces.Application.Features.Items.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Mappings
{
    public class MeaasureUnitProfile : Profile
    {
        public MeaasureUnitProfile()
        {
            CreateMap<MeasureUnitsResponse, Models.MeasureUnits>().ReverseMap();
        }
    }
}
