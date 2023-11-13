using Forces.Application.Features.RackStore.Commands.AddEdit;
using Forces.Application.Features.RackStore.Queries.GetAll;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.RackStore
{
    public interface IRackStoreManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditRackStoreCommand command);
        Task<IResult<int>> DeleteAsync(int Id);
        Task<IResult<List<GetAllRackStoreResponse>>> GetAllAsync();
    }
}
