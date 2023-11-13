using AutoMapper;
using Forces.Application.Features.Products.Commands.AddEdit;
using Forces.Domain.Entities.Catalog;

namespace Forces.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddEditProductCommand, Product>().ReverseMap();
        }
    }
}