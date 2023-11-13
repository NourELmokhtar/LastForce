using Forces.Application.Features.Documents.Commands.AddEdit;
using Forces.Application.Features.Documents.Queries.GetAll;
using Forces.Application.Features.Documents.Queries.GetById;
using Forces.Application.Requests.Documents;
using Forces.Shared.Wrapper;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Misc.Document
{
    public interface IDocumentManager : IManager
    {
        Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request);

        Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(GetDocumentByIdQuery request);

        Task<IResult<int>> SaveAsync(AddEditDocumentCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}