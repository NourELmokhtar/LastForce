using Forces.Application.Features.SectionStore.Commands.AddEdit;
using Forces.Application.Features.SectionStore.Queries.GetAll;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.StoreSection
{
    public interface IStoreSectionManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditSectionStoreCommand command);
        Task<IResult<int>> DeleteAsync(int Id);
        Task<IResult<List<GetAllSectionStoreResponse>>> GetAllAsync();
    }
}
