using Forces.Application.Features.AirKind.Commands.AddEdit;
using Forces.Application.Features.AirKind.Queries.GetAll;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.AirKind
{
    public interface IAirKindManager :IManager
    {
        Task<IResult<int>> SaveAsync(AddEditAirKindCommand command);
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<GetAllAirKindResponse>> GetByIdAsync(int id);
        Task<IResult<List<GetAllAirKindResponse>>> GetAllAsync();
        Task<IResult<List<GetAllAirKindResponse>>> GetAllByTypeIdAsync(int id);


    }
}
