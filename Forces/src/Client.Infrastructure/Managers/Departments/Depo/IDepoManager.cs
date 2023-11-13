using Forces.Application.Features.DepoDepartment.Commands.AddEdit;
using Forces.Application.Features.DepoDepartment.Queries.GetAll;
using Forces.Application.Features.DepoDepartment.Queries.GetByForceId;
using Forces.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Departments.Depo
{
    public interface IDepoManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditDepoCommand request);
        Task<IResult<List<GetAllDepoDepartmentsResponse>>> GetAllAsync();
        Task<IResult<List<GetAllDepoDepartmentsResponse>>> GetAllDepoAsync();
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<List<GetAllDepoByForceIdResponse>>> GetByForceIdAsync(int ForceId);
    }
}
