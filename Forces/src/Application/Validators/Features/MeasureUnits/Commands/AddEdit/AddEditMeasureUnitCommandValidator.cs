using FluentValidation;
using Forces.Application.Features.MeasureUnits.Commands.AddEdit;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Validators.Features.MeasureUnits.Commands.AddEdit
{
    public class AddEditMeasureUnitCommandValidator : AbstractValidator<AddEditMeasureUnitsCommand>
    {
        public AddEditMeasureUnitCommandValidator(IStringLocalizer<AddEditMeasureUnitCommandValidator> localizer)
        {
            RuleFor(request => request.MeasureName).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Unit Name is required!"]);
        }
    }
}
