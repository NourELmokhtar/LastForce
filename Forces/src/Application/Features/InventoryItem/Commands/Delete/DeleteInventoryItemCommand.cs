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

namespace Forces.Application.Features.InventoryItem.Commands.Delete
{
    public class DeleteInventoryItemCommand : IRequest<IResult<int>>
    {
        [Required]
        public int InventoryItemId { get; set; }
    }
    internal class DeleteInventoryItemCommandHandler : IRequestHandler<DeleteInventoryItemCommand, IResult<int>>
    {
        private readonly IStringLocalizer<DeleteInventoryItemCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteInventoryItemCommandHandler(IStringLocalizer<DeleteInventoryItemCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<int>> Handle(DeleteInventoryItemCommand command, CancellationToken cancellationToken)
        {
            var Item = await _unitOfWork.Repository<Models.InventoryItem>().GetByIdAsync(command.InventoryItemId);
            if (Item != null)
            {
                await _unitOfWork.Repository<Models.InventoryItem>().DeleteAsync(Item);
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
