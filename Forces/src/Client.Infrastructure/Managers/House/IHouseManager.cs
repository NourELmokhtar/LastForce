using Forces.Application.Features.House.Commands.AddEdit;
using Forces.Application.Features.House.Queries.GetAll;
using Forces.Application.Features.House.Queries.GetBySpecifics;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.House
{
    public interface IHouseManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditHouseCommand request);
        Task<IResult<List<GetAllHousesResponse>>> GetAllAsync();
        Task<IResult<GetHouseByResponse>> GetHouseByAsync(int Id);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
