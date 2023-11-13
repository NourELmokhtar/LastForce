using Forces.Application.Features.AirCraft.Commands.AddEdit;
using Forces.Application.Features.AirCraft.Queries.GetAll;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.AirCraft
{
    public interface IAirCraftManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditAirCraftCommand command);
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<GetAllAirCraftResponse>> GetByIdAsync(int id);
        Task<IResult<List<GetAllAirCraftResponse>>> GetAllAsync();
        Task<IResult<List<GetAllAirCraftResponse>>> GetAllByKindIdAsync(int KindId);


    }
}
