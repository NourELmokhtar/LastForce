using FluentValidation;
using Forces.Application.Features.PersonalItems.Commands.AddEdit;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Validators.Features.PersonalItems.Commands.AddEdit
{
    public class AddEditPersonalItemCommandValidator : AbstractValidator<AddEditPersonalItemCommand>
    {
        public AddEditPersonalItemCommandValidator(IStringLocalizer<AddEditPersonalItemCommandValidator> _localizer)
        {
            RuleFor(x => x.BaseId).NotNull().NotEqual(0).WithMessage(_localizer["Base is Required!"]);
            RuleFor(x => x.ItemName).NotNull().NotEmpty().WithMessage(_localizer["Item Name is Required!"]);
            RuleFor(x => x.ItemCode).NotNull().NotEmpty().WithMessage(_localizer["Item Code is Required!"]);
            RuleFor(x => x.ItemNsn).NotNull().NotEmpty().WithMessage(_localizer["Serial Number is Required!"]);
            When(x => !x.StorageableItem, () =>
            {
                RuleFor(x => x.ItemPrice).NotNull().NotEmpty().WithMessage(_localizer["You Must Provide A Price"]);
                RuleFor(x => x.TailerId).NotNull().NotEmpty().NotEqual(0).WithMessage(_localizer["You Must Provide A Tailer For This Item"]);
            });
        }
    }
}
