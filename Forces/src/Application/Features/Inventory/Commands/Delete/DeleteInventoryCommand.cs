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

namespace Forces.Application.Features.Inventory.Commands.Delete
{
    public class DeleteInventoryCommand : IRequest<IResult<int>>
    {
        [Required]
        public int InventoryId { get; set; }
    }

    internal class DeleteInventoryCommandHandler : IRequestHandler<DeleteInventoryCommand, IResult<int>>
    {
        private readonly IStringLocalizer<DeleteInventoryCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteInventoryCommandHandler(
            IStringLocalizer<DeleteInventoryCommandHandler> localizer,
            IUnitOfWork<int> unitOfWork)
        {
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<int>> Handle(DeleteInventoryCommand request, CancellationToken cancellationToken)
        {
            var Inventory = await _unitOfWork.Repository<Models.Inventory>().GetByIdAsync(request.InventoryId);
            if (Inventory != null)
            {
                await _unitOfWork.Repository<Models.Inventory>().DeleteAsync(Inventory);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(Inventory.Id, _localizer["Inventory Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Inventory Not Found!"]);
            }
        }
    }
}
