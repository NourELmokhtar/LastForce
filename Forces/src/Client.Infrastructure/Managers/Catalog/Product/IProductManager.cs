using Forces.Application.Features.Products.Commands.AddEdit;
using Forces.Application.Features.Products.Queries.GetAllPaged;
using Forces.Application.Requests.Catalog;
using Forces.Shared.Wrapper;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Catalog.Product
{
    public interface IProductManager : IManager
    {
        Task<PaginatedResult<GetAllPagedProductsResponse>> GetProductsAsync(GetAllPagedProductsRequest request);

        Task<IResult<string>> GetProductImageAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditProductCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}