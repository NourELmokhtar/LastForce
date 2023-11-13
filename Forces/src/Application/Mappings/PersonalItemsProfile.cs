using AutoMapper;
using Forces.Application.Features.PersonalItems.Commands.AddEdit;
using Forces.Application.Features.PersonalItems.Queries.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Mappings
{
    public class PersonalItemsProfile : Profile
    {
        public PersonalItemsProfile()
        {
            CreateMap<AddEditPersonalItemCommand, Models.PersonalItems>().ReverseMap();
            CreateMap<PersonalItemDto, Models.PersonalItems>().ReverseMap();
        }
    }
}
