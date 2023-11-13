using Forces.Application.Features.Tailers.Commands.AddEdit;
using Forces.Application.Features.Tailers.Queries;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Tailers
{
    public interface ITailersManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditTailerCommand request);
        Task<IResult<List<TailerDto>>> GetAllAsync();
        Task<IResult<int>> DeleteAsync(int id);

    }
}
