using AutoMapper;
using Forces.Application.Features.DocumentTypes.Commands.AddEdit;
using Forces.Application.Features.DocumentTypes.Queries.GetAll;
using Forces.Application.Features.DocumentTypes.Queries.GetById;
using Forces.Domain.Entities.Misc;

namespace Forces.Application.Mappings
{
    public class DocumentTypeProfile : Profile
    {
        public DocumentTypeProfile()
        {
            CreateMap<AddEditDocumentTypeCommand, DocumentType>().ReverseMap();
            CreateMap<GetDocumentTypeByIdResponse, DocumentType>().ReverseMap();
            CreateMap<GetAllDocumentTypesResponse, DocumentType>().ReverseMap();
        }
    }
}