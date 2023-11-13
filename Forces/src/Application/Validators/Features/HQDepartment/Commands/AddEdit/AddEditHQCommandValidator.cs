using FluentValidation;
using Forces.Application.Features.HQDepartment.Commands.AddEdit;
using Microsoft.Extensions.Localization;

namespace Forces.Application.Validators.Features.HQDepartment.Commands.AddEdit
{
    public class AddEditHQCommandValidator : AbstractValidator<AddEditHQCommand>
    {
        public AddEditHQCommandValidator(IStringLocalizer<AddEditHQCommandValidator> localizer)
        {
            RuleFor(request => request.Name).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Department Name is required!"]);
        }
    }
}
