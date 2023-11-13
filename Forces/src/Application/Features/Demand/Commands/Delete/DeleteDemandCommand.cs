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

namespace Forces.Application.Features.Demand.Commands.Delete
{
    public class DeleteDemandCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

    }
    internal class DeleteDemandCommandHandler : IRequestHandler<DeleteDemandCommand, Result<int>>
    {

        private readonly IStringLocalizer<DeleteDemandCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteDemandCommandHandler(IStringLocalizer<DeleteDemandCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {

            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeleteDemandCommand command, CancellationToken cancellationToken)
        {

            var isDemandUsed = await _unitOfWork.Repository<Models.Demand>().Entities.AnyAsync(x => x.Id == command.Id);
            if (!isDemandUsed)
            {
                var demand = await _unitOfWork.Repository<Models.Demand>().GetByIdAsync(command.Id);
                if (demand != null)
                {
                    await _unitOfWork.Repository<Models.Demand>().DeleteAsync(demand);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(demand.Id, _localizer["Demand Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Demand Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["This Demand Inuse,Deletion Not Allowed"]);
            }

        }
    }

}