using Forces.Application.Features.InventoryInventoryItem.Commands.AddEdit;
using Forces.Application.Features.InventoryItem.Queries.GetAll;
using Forces.Application.Features.InventoryItem.Queries.GetBySpecifics;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.InventoryItem
{
    public interface IInventoryItemManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditInventoryItemCommand request);
        Task<IResult<List<GetAllInventoryItemsResponse>>> GetAllAsync();
        Task<IResult<GetInventoryItemByResponse>> GetInventoryItemByAsync(int Id);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
