using FluentValidation;
using Forces.Application.Features.Items.Commands.AddEdit;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Validators.Features.Items.Commands.AddEdit
{
    public class AddEditItemCommandValidator : AbstractValidator<AddEditItemCommand>
    {
        public AddEditItemCommandValidator(IStringLocalizer<AddEditItemCommandValidator> localizer)
        {
            RuleFor(request => request.ItemName).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Item Name is required!"]);
            RuleFor(request => request.ItemCode).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Item Code is required!"]);
            RuleFor(request => request.ItemNsn).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Item NSN is required!"]);
            RuleFor(request => request.VoteCodesId).NotNull().Must(x => x != 0).WithMessage(x => localizer["Vote Code is required!"]);
            RuleFor(request => request.MeasureUnitId).NotNull().Must(x => x != 0).WithMessage(x => localizer["Measure Unit is required!"]);
            When(request => request.ItemClass == Enums.ItemClass.A || request.ItemClass == Enums.ItemClass.B, () =>
            {
                RuleFor(request => request.SerialNumber).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Serial Number Is Required!"]);
            });

        }
    }
}
