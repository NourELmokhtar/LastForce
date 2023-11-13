using FluentValidation;
using Forces.Application.Features.BaseSections.Commands.AddEdit;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Validators.Features.BaseSections.Commands.AddEdit
{
    public class AddEditBaseSectionCommandValidator : AbstractValidator<AddEditBaseSectionCommand>
    {
        public AddEditBaseSectionCommandValidator(IStringLocalizer<AddEditBaseSectionCommandValidator> localizer)
        {
            RuleFor(request => request.SectionName).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Section Name is required!"]);
            RuleFor(request => request.SectionCode).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Section Code is required!"]);
            RuleFor(request => request.BaseId).Must(x => !string.IsNullOrWhiteSpace(x.ToString())).WithMessage(x => localizer["Section Base is required!"]);
        }
    }
}
