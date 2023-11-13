using Forces.Application.Features.PersonalItems.Commands.AddEdit;
using Forces.Application.Features.PersonalItems.Queries.DTO;
using Forces.Application.Features.PersonalItems.Queries.GetByFillter;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.PersonalItems
{
    public interface IPersonalItemsManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditPersonalItemCommand request);
        Task<IResult<List<PersonalItemDto>>> GetAllAsync();
        Task<IResult<PersonalItemDto>> GetByIdAsync(int Id);
        Task<IResult<List<PersonalItemDto>>> GetAllByConditionsAsync(GetPersonalItemsByFillter ConditionsModel);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
