using AutoMapper;
using Forces.Application.Features.PersonalItemOperations.Commands.AddEdit;
using Forces.Application.Features.PersonalItemOperations.Queries.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Mappings
{
    public class PersonalItemOperations : Profile
    {
        public PersonalItemOperations()
        {
            CreateMap<OperationDetailsDto, Models.PersonalItemsOperation_Details>().ReverseMap();
            CreateMap<PersonalItemOperationDetail, Models.PersonalItemsOperation_Details>().ReverseMap();
            CreateMap<AddEditPersonalItemOperationCommand, Models.PersonalItemsOperation_Hdr>().ReverseMap();
            CreateMap<PersonalItemOperationDto, Models.PersonalItemsOperation_Hdr>().ReverseMap();
        }
    }
}
