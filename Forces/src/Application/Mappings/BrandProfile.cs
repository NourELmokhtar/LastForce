using AutoMapper;
using Forces.Application.Features.Brands.Commands.AddEdit;
using Forces.Application.Features.Brands.Queries.GetAll;
using Forces.Application.Features.Brands.Queries.GetById;
using Forces.Domain.Entities.Catalog;

namespace Forces.Application.Mappings
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {

            CreateMap<GetBrandByIdResponse, Brand>().ReverseMap();
            CreateMap<GetAllBrandsResponse, Brand>().ReverseMap();
        }
    }
}