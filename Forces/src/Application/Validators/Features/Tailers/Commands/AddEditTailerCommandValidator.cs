using FluentValidation;
using Forces.Application.Features.Tailers.Commands.AddEdit;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Validators.Features.Tailers.Commands
{
    public class AddEditTailerCommandValidator : AbstractValidator<AddEditTailerCommand>
    {
        public AddEditTailerCommandValidator(IStringLocalizer<AddEditTailerCommandValidator> localizer)
        {
            RuleFor(request => request.Name).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Tailer Name is required!"]);
            RuleFor(request => request.BaseId).NotNull().NotEqual(0).WithMessage(x => localizer["Tailer's Base is required!"]);
        }
    }
}
