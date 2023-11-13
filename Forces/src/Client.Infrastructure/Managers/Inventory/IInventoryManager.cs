using Forces.Application.Features.Bases.Commands.AddEdit;
using Forces.Application.Features.Bases.Queries.GetAll;
using Forces.Application.Features.Bases.Queries.GetByForceId;
using Forces.Application.Features.Bases.Queries.GetById;
using Forces.Application.Features.Inventory.Commands.AddEdit;
using Forces.Application.Features.Inventory.Queries.GetAll;
using Forces.Application.Features.Inventory.Queries.GetBySpecifics;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Inventory
{
    public interface IInventoryManager :IManager
    {
        Task<IResult<int>> SaveAsync(AddEditInventoryCommand request);
        Task<IResult<List<GetAllInventoriesResponse>>> GetAllAsync();
        Task<IResult<GetInventoryByResponse>> GetInventoryByIdAsync(int Id);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
