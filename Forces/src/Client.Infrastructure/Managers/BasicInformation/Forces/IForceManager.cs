using Forces.Application.Features.Forces.Commands.AddEdit;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Application.Features.Forces.Queries.GetById;
using Forces.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.BasicInformation.Forces
{
    public interface IForceManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditForceCommand request);
        Task<IResult<List<GetAllForcesResponse>>> GetAllAsync();
        Task<IResult<GetForceByIdResponse>> GetForceByIdAsync(int Id);
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<List<GetAllForcesResponse>>> GetAllForcesAsync();
    }
}
