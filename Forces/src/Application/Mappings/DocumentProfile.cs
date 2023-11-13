using AutoMapper;
using Forces.Application.Features.Documents.Commands.AddEdit;
using Forces.Application.Features.Documents.Queries.GetById;
using Forces.Domain.Entities.Misc;

namespace Forces.Application.Mappings
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<AddEditDocumentCommand, Document>().ReverseMap();
            CreateMap<GetDocumentByIdResponse, Document>().ReverseMap();
        }
    }
}