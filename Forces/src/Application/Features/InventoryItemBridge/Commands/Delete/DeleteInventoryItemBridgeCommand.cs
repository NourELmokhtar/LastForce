using Forces.Application.Features.InventoryItem.Commands.Delete;
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

namespace Forces.Application.Features.InventoryItemBridge.Commands.Delete
{
    public class DeleteInventoryItemBridgeCommand:IRequest<IResult<int>>
    {
        
        [Required]
        public int InventoryItemBridgeId { get; set; }
    }
    internal class DeleteInventoryItemCommandHandler : IRequestHandler<DeleteInventoryItemBridgeCommand, IResult<int>>
    {
        private readonly IStringLocalizer<DeleteInventoryItemCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteInventoryItemCommandHandler(IStringLocalizer<DeleteInventoryItemCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<int>> Handle(DeleteInventoryItemBridgeCommand command, CancellationToken cancellationToken)
        {
            var Item = await _unitOfWork.Repository<Models.InventoryItemBridge>().GetByIdAsync(command.InventoryItemBridgeId);
            if (Item != null)
            {
                await _unitOfWork.Repository<Models.InventoryItemBridge>().DeleteAsync(Item);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(Item.Id, _localizer["InventoryItemBridge Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Item Not Found!"]);
            }
        }
    }
}
