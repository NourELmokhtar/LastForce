using FluentValidation;
using Forces.Application.Features.AirType.Commands.AddEdit;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Validators.Features.AirTypes.Commands.AddEdit
{
    public class AddEditAirTypeCommandValidator : AbstractValidator<AddEditAirTypeCommand>
    {
        public AddEditAirTypeCommandValidator(IStringLocalizer<AddEditAirTypeCommandValidator> localizer)
        {
            RuleFor(x => x.AirTypeCode).NotEmpty().WithMessage(localizer["Type Code Is Required"]);
            RuleFor(x => x.AirTypeName).NotEmpty().WithMessage(localizer["Type Name Is Required"]);
           
        }
    }
}
