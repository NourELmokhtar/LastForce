using Forces.Application.Features.InventoryInventoryItem.Commands.AddEdit;
using Forces.Application.Features.InventoryItem.Queries.GetAll;
using Forces.Application.Features.InventoryItem.Queries.GetBySpecifics;
using Forces.Application.Features.InventoryItemBridge.Commands.AddEdit;
using Forces.Application.Features.InventoryItemBridge.Queries.GetAll;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.InventoryItemBridge
{
    public interface IInventoryItemBridgeManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditInventoryItemBridgeCommand request);
        Task<IResult<List<GetAllInventoryItemBridgeResponse>>> GetAllAsync();
        Task<IResult<GetAllInventoryItemBridgeResponse>> GetInventoryItemByAsync(int Id);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
