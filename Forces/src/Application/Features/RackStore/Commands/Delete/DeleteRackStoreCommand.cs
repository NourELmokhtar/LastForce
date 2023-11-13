using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.RackStore.Commands.Delete
{
    public class DeleteRackStoreCommand : IRequest<Result<int>>
    {
        public int RackCode { get; set; }
    }
    internal class DeleteRackStoreCommandHandler : IRequestHandler<DeleteRackStoreCommand, Result<int>>
    {

        private readonly IStringLocalizer<DeleteRackStoreCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteRackStoreCommandHandler(IStringLocalizer<DeleteRackStoreCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {

            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteRackStoreCommand command, CancellationToken cancellationToken)
        {

            var isRackStoreUsed = await _unitOfWork.Repository<Models.ShilfRack>().Entities.AnyAsync(x => x.RackId == command.RackCode);
            if (!isRackStoreUsed)
            {
                var rack = await _unitOfWork.Repository<Models.RackStore>().GetByIdAsync(command.RackCode);
                if (rack != null)
                {
                    await _unitOfWork.Repository<Models.RackStore>().DeleteAsync(rack);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(rack.Id, _localizer["Rack Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Rack Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Rack Inused , Deleation Not Allowed!"]);
            }
        }
    }
}