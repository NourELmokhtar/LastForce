using FluentValidation;
using Forces.Application.Features.Vehicle.Commands.AddEdit;
using Forces.Application.Validators.Features.Tailers.Commands;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Validators.Features.Vehicles.AddEdit
{
    public class AddEditVehicleCommandValidator : AbstractValidator<AddEditVehicleCommand>
    {
        public AddEditVehicleCommandValidator(IStringLocalizer<AddEditVehicleCommandValidator> localizer)
        {
            RuleFor(x => x.VehicleName).NotEmpty().WithMessage(localizer["Vehicle Name Can not be Empty!"]);
            RuleFor(x => x.DateOfEnter).NotNull().WithMessage(localizer["Date Of Enter Can not be Empty!"]);
            RuleFor(x => x.EngineNo).NotEmpty().WithMessage(localizer["Engine Number Can not be Empty!"]);
            RuleFor(x => x.WheelsCount).NotEmpty().WithMessage(localizer["Wheels Count Can not be Empty!"]);
            RuleFor(x => x.MadeIn).NotEmpty().WithMessage(localizer["Factory Details Can not be Empty!"]);
            RuleFor(x => x.VehicleNumber).NotEmpty().WithMessage(localizer["Vehicle Number Can not be Empty!"]);
            RuleFor(x => x.WheelsSize).NotEmpty().WithMessage(localizer["Wheels Size Can not be Empty!"]);
            RuleFor(x => x.WheelsYear).NotEmpty().WithMessage(localizer["Wheels Year Can not be Empty!"]);
            RuleFor(x => x.Year).NotEmpty().WithMessage(localizer["Model Year Can not be Empty!"]);
        }
    }
}
