using FluentValidation;
using Forces.Application.Requests.VoteCodes;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Validators.Requests.VoteCodes
{
    public class AddEditVoteCodeRequestValidator : AbstractValidator<AddEditVoteCodeRequest>
    {
        public AddEditVoteCodeRequestValidator(IStringLocalizer<AddEditVoteCodeRequestValidator> localizer)
        {
            RuleFor(request => request.VoteCode).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Vote Code is required!"]);
            RuleFor(request => request.VoteShortcut).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Shortcut is required!"]);
        }
    }
}
