using FluentValidation;
using Forces.Application.Features.DepoDepartment.Commands.AddEdit;
using Microsoft.Extensions.Localization;

namespace Forces.Application.Validators.Features.DepoDepartment.Commands.AddEdit
{
    public class AddEditDepoCommandValidator : AbstractValidator<AddEditDepoCommand>
    {
        public AddEditDepoCommandValidator(IStringLocalizer<AddEditDepoCommandValidator> localizer)
        {
            RuleFor(request => request.Name).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Department Name is required!"]);
        }
    }
}
