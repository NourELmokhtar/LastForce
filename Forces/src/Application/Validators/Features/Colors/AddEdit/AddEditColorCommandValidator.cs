using FluentValidation;
using Forces.Application.Features.Color.Commands.AddEdit;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Validators.Features.Colors.AddEdit
{
    public class AddEditColorCommandValidator : AbstractValidator<AddEditColorCommand>
    {
        public AddEditColorCommandValidator(IStringLocalizer<AddEditColorCommandValidator>localizer)
        {
            RuleFor(x => x.ColorName).NotEmpty().WithMessage(localizer["Color Name Can not be Empty"]);
        }
    }
}
