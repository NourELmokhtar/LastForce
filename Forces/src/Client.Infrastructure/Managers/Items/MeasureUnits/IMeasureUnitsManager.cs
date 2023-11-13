using Forces.Application.Features.MeasureUnits.Commands.AddEdit;
using Forces.Application.Features.MeasureUnits.Queries.GetAll;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Items.MeasureUnits
{
    public interface IMeasureUnitsManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditMeasureUnitsCommand request);
        Task<IResult<List<GetAllMeasureUnitsResponse>>> GetAllAsync();
        Task<IResult<int>> DeleteAsync(int id);
    }
}
