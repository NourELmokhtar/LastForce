using AutoMapper;
using FluentValidation;
using Forces.Application.Features.Forces.Commands.AddEdit;
using Forces.Application.Interfaces.Repositories;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Validators.Features.Forces.Commands.AddEdit
{
    public class AddEditForceCommandValidator : AbstractValidator<AddEditForceCommand>
    {


        public AddEditForceCommandValidator(IStringLocalizer<AddEditForceCommandValidator> localizer)
        {
            RuleFor(request => request.ForceName).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Force Name is required!"]);
            RuleFor(request => request.ForceCode).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Force Code is required!"]);
        }

    }
}
