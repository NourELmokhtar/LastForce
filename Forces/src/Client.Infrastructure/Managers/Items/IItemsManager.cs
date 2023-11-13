using Forces.Application.Features.Items.Commands.AddEdit;
using Forces.Application.Features.Items.Queries.GetAll;
using Forces.Application.Features.Items.Queries.GetBySpecifics;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Items
{
    public interface IItemsManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditItemCommand request);
        Task<IResult<List<GetAllItemsResponse>>> GetAllAsync();
        Task<IResult<List<GetItemsByResponse>>> GetAllByConditionsAsync(GetAllItemsBy ConditionsModel);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
