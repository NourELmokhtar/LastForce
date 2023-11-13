using FluentValidation;
using Forces.Application.Features.AirCraft.Commands.AddEdit;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Validators.Features.AirCrafts.Commands
{
    public class AddEditAirCraftCommandValidator : AbstractValidator<AddEditAirCraftCommand>
    {
        public AddEditAirCraftCommandValidator(IStringLocalizer<AddEditAirCraftCommandValidator> localizer)
        {
            RuleFor(x => x.AirCraftCode).NotEmpty().WithMessage(localizer["Code Is Required"]);
            RuleFor(x=>x.AirCraftName).NotEmpty().WithMessage(localizer["Name Is Required"]);
            RuleFor(x=>x.AirKindId).NotNull().WithMessage(localizer["Air Kind Is Required"]);
            RuleFor(x=>x.BaseId).NotNull().WithMessage(localizer["Base Is Required"]);
            RuleFor(x => x.MadeIN).NotEmpty().WithMessage(localizer["Made In Is Required"]);
            RuleFor(x => x.SectionId).NotNull().WithMessage(localizer["Section Is Required"]);
        }
    }
}
