using Forces.Application.Features.BaseSections.Commands.AddEdit;
using Forces.Application.Features.BaseSections.Queries.GetAll;
using Forces.Application.Features.BaseSections.Queries.GetByBaseId;
using Forces.Application.Features.BaseSections.Queries.GetById;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.BasicInformation.BaseSections
{
    public interface IBaseSectionManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditBaseSectionCommand request);
        Task<IResult<List<GetAllBasesSectionsQueryResponse>>> GetAllAsync();
        Task<IResult<GetBaseSectionByIdQueryResponse>> GetBaseSectionByIdAsync(int Id);
        Task<IResult<List<GetAllSectionsByBaseIdQueryResponse>>> GetSectionsByBaseIdAsync(int BaseId);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
