using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.MeasureUnits.Commands.Delete
{
    public class DeleteBinRackCmmand : IRequest<IResult<int>>
    {
        [Required]
        public int Id { get; set; }
    }
    internal class DeleteMeasureUnitCommandHandler : IRequestHandler<DeleteBinRackCmmand, IResult<int>>
    {
        private protected IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteMeasureUnitCommandHandler> _localizer;

        public DeleteMeasureUnitCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteMeasureUnitCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<IResult<int>> Handle(DeleteBinRackCmmand command, CancellationToken cancellationToken)
        {
            var unit = await _unitOfWork.Repository<Models.MeasureUnits>().Entities.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == command.Id);
            if (unit != null)
            {
                if (unit.Items.Count > 0)
                {
                    return await Result<int>.FailAsync(_localizer["Deletion Not Allowed Unless Delete All Related Items!"]);
                }
                await _unitOfWork.Repository<Models.MeasureUnits>().DeleteAsync(unit);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(unit.Id, _localizer["Measure Unit Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Measure Unit Not Found!"]);
            }

        }
    }
}
