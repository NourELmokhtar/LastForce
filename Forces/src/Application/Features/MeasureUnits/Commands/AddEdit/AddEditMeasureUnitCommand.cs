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

namespace Forces.Application.Features.MeasureUnits.Commands.AddEdit
{
    public class AddEditMeasureUnitsCommand : IRequest<IResult<int>>
    {
        public int Id { get; set; }
        [Required]
        public string MeasureName { get; set; }

    }
    internal class AddEditMeasureUnitCommandHandler : IRequestHandler<AddEditMeasureUnitsCommand, IResult<int>>
    {
        private protected IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditMeasureUnitCommandHandler> _localizer;

        public AddEditMeasureUnitCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<AddEditMeasureUnitCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<IResult<int>> Handle(AddEditMeasureUnitsCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                if (await _unitOfWork.Repository<Models.MeasureUnits>().Entities.AnyAsync(x => x.Name == command.MeasureName))
                {
                    return await Result<int>.FailAsync(_localizer["Measure Unit {0} is Already Exist!", command.MeasureName]);
                }
                Models.MeasureUnits measure = new Models.MeasureUnits() { Name = command.MeasureName };
                await _unitOfWork.Repository<Models.MeasureUnits>().AddAsync(measure);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(measure.Id, _localizer["Measure Unit Added!"]);
            }
            else
            {
                var unit = await _unitOfWork.Repository<Models.MeasureUnits>().GetByIdAsync(command.Id);
                if (unit != null)
                {
                    if (unit.Name != command.MeasureName)
                    {
                        if (await _unitOfWork.Repository<Models.MeasureUnits>().Entities.AnyAsync(x => x.Name == command.MeasureName))
                        {
                            return await Result<int>.FailAsync(_localizer["Measure Unit {0} is Already Exist!", command.MeasureName]);
                        }
                    }
                    unit.Name = command.MeasureName ?? unit.Name;
                    await _unitOfWork.Repository<Models.MeasureUnits>().UpdateAsync(unit);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(unit.Id, _localizer["Measure Unit Updated!"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Measure Unit Not Found!"]);
                }
            }

        }
    }
}
