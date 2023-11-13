using Forces.Application.Features.Building.Commands.AddEdit;
using Forces.Application.Features.Building.Queries.GetAll;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Building
{
    public interface IBuildingManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditBuildingCommand command);
        Task<IResult<int>> DeleteAsync(int Id);
        Task<IResult<List<GetAllBuildingsResponse>>> GetAllAsync();
    }
}
