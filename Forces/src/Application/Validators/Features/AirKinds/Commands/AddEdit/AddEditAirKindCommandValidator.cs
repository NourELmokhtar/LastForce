using FluentValidation;
using Forces.Application.Features.AirKind.Commands.AddEdit;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Validators.Features.AirKinds.Commands.AddEdit
{
    public class AddEditAirKindCommandValidator : AbstractValidator<AddEditAirKindCommand>
    {
        public AddEditAirKindCommandValidator(IStringLocalizer<AddEditAirKindCommandValidator> localizer)
        {
            RuleFor(x => x.AirKindCode).NotEmpty().WithMessage(localizer["Kind Code Is Required"]);
            RuleFor(x=>x.AirKindName).NotEmpty().WithMessage(localizer["Kind Name Is Required"]);
            RuleFor(x=>x.AirTypeId).NotNull().WithMessage(localizer["Kind Type Is Required"]);
        }
    }
}
