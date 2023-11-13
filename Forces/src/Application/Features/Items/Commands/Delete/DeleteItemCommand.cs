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

namespace Forces.Application.Features.Items.Commands.Delete
{
    public class DeleteItemCommand : IRequest<IResult<int>>
    {
        [Required]
        public int ItemId { get; set; }
    }
    internal class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, IResult<int>>
    {
        private readonly IStringLocalizer<DeleteItemCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteItemCommandHandler(IStringLocalizer<DeleteItemCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<int>> Handle(DeleteItemCommand command, CancellationToken cancellationToken)
        {
            var Item = await _unitOfWork.Repository<Models.Items>().GetByIdAsync(command.ItemId);
            if (Item != null)
            {
                await _unitOfWork.Repository<Models.Items>().DeleteAsync(Item);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(Item.Id, _localizer["Item Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Item Not Found!"]);
            }
        }
    }
}
