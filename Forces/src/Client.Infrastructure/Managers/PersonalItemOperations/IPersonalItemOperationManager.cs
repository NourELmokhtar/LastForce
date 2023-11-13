using Forces.Application.Features.PersonalItemOperations.Commands.AddEdit;
using Forces.Application.Features.PersonalItemOperations.Queries.Dto;
using Forces.Application.Features.PersonalItemOperations.Queries.Eligibility;
using Forces.Application.Features.PersonalItemOperations.Queries.GetByFillter;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.PersonalItemOperations
{
    public interface IPersonalItemOperationManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditPersonalItemOperationCommand request);
        Task<IResult<List<PersonalItemOperationDto>>> GetAllAsync(GetPersonalItemsOperationsByFillter request);
        Task<IResult<List<PersonalItemOperationDto>>> GetAllFillterAsync(GetPersonalItemsOperationsByFillter request);
        Task<IResult<EligibilityModel>> Check(ItemEligibilityQuery Model);

    }
}
