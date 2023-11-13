using Forces.Application.Features.AirType.Commands.AddEdit;
using Forces.Application.Features.AirType.Queries.GetAll;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.AirType
{
    public interface IAirTypeManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditAirTypeCommand command);
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<GetAllAirTypeResponse>> GetByIdAsync(int id);
        Task<IResult<List<GetAllAirTypeResponse>>> GetAllAsync();

    }
}
