using AutoMapper;
using Forces.Application.Features.Items.Commands.AddEdit;
using Forces.Application.Features.Items.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Mappings
{
    public class temsProfile : Profile
    {
        public temsProfile()
        {
            CreateMap<AddEditItemCommand, Models.Items>().ReverseMap();
            CreateMap<GetAllItemsResponse, Models.Items>().ReverseMap();
        }
    }
}
