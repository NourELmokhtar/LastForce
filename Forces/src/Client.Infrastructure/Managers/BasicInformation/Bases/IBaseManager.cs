using Forces.Application.Features.Bases.Commands.AddEdit;
using Forces.Application.Features.Bases.Queries.GetAll;
using Forces.Application.Features.Bases.Queries.GetByForceId;
using Forces.Application.Features.Bases.Queries.GetById;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.BasicInformation.Bases
{
    public interface IBaseManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditBaseCommand request);
        Task<IResult<List<GetAllBasesResponse>>> GetAllAsync();
        Task<IResult<GetBaseByIdResponse>> GetBaseByIdAsync(int Id);
        Task<IResult<List<GetAllBasesByForceIdResponse>>> GetBasesByForceIdAsync(int ForceId);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
