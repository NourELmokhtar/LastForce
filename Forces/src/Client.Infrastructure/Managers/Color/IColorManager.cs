using Forces.Application.Features.Color.Commands.AddEdit;
using Forces.Application.Features.Color.Queries.GetAll;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Color
{
    public interface IColorManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditColorCommand command);
        Task<IResult<int>> DeleteAsync(int Id);
        Task<IResult<List<GetAllColorResponse>>> GetAllAsync();
    }
}
