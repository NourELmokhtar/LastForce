using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.PersonalItems.Commands.Delete
{
    public class DeletePersonalItemCommand : IRequest<IResult<int>>
    {
        [Required]
        public int PersonalItemId { get; set; }
    }
    internal class DeletePersonalItemCommandHandler : IRequestHandler<DeletePersonalItemCommand, IResult<int>>
    {
        private readonly IStringLocalizer<DeletePersonalItemCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        public async Task<IResult<int>> Handle(DeletePersonalItemCommand command, CancellationToken cancellationToken)
        {
            var PersonalItem = await _unitOfWork.Repository<Models.PersonalItems>().GetByIdAsync(command.PersonalItemId);
            if (PersonalItem != null)
            {
                await _unitOfWork.Repository<Models.PersonalItems>().DeleteAsync(PersonalItem);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(PersonalItem.Id, _localizer["Personal Item Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Personal Item Not Found!"]);
            }
        }
    }
}
