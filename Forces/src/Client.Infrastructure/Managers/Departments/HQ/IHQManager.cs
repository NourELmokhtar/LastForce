using Forces.Application.Features.HQDepartment.Commands.AddEdit;
using Forces.Application.Features.HQDepartment.Queries.GetAll;
using Forces.Application.Features.HQDepartment.Queries.GetByForceId;
using Forces.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Departments.HQ
{
    public interface IHQManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditHQCommand request);
        Task<IResult<List<GetAllHQDepartmentsResponse>>> GetAllAsync();
        Task<IResult<List<GetAllHQDepartmentsResponse>>> GetAllHqAsync();
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<List<GetAllHQbyForceIdResponse>>> GetByForceIdAsync(int ForceId);
    }
}
