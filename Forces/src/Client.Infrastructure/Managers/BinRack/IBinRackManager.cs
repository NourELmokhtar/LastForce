using Forces.Application.Features.BinRack.Commands.AddEdit;
using Forces.Application.Features.BinRack.Queries.GetAll;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.BinRack
{
    public interface IBinRackManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditBinRackCommand command);
        Task<IResult<int>> DeleteAsync(int Id);
        Task<IResult<List<GetAllBinRackResponse>>> GetAllAsync();
    }
}
