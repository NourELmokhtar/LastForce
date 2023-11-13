using FluentValidation;
using Forces.Application.Features.Bases.Commands.AddEdit;
using Microsoft.Extensions.Localization;

namespace Forces.Application.Validators.Features.Bases.Commands.AddEdit
{
    public class AddEditBaseCommandValidator : AbstractValidator<AddEditBaseCommand>
    {
        public AddEditBaseCommandValidator(IStringLocalizer<AddEditBaseCommandValidator> localizer)
        {
            RuleFor(request => request.BaseName).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Base Name is required!"]);
            RuleFor(request => request.BaseCode).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Base Code is required!"]);
        }
    }
}
