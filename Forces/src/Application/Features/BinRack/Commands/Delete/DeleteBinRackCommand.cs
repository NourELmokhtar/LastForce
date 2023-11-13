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

namespace Forces.Application.Features.BinRack.Commands.Delete
{
    public class DeleteBinRackCommand : IRequest<Result<int>>

    {
        public int BinCode { get; set; }

    }
    internal class DeleteBinRackCommandHandler : IRequestHandler<DeleteBinRackCommand, Result<int>>
    {

        private readonly IStringLocalizer<DeleteBinRackCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        private object command;

        public DeleteBinRackCommandHandler(IStringLocalizer<DeleteBinRackCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {

            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }


        public async Task<Result<int>> Handle(DeleteBinRackCommand command, CancellationToken cancellationToken)
        {
            var isBinUsed = await _unitOfWork.Repository<Models.BinRack>().Entities.AnyAsync(x => x.BinCode == command.BinCode);
            if (!isBinUsed)
            {
                var Bin = await _unitOfWork.Repository<Models.BinRack>().GetByIdAsync(command.BinCode);
                if (Bin != null)
                {
                    await _unitOfWork.Repository<Models.BinRack>().DeleteAsync(Bin);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(Bin.Id, _localizer["Bin Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Bin Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["This Bin Inuse,Deletion Not Allowed"]);
            }
        }
    }
}
