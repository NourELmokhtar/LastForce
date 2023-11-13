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
    public class AddEditVcodeTransactionRequestValidator : AbstractValidator<AddEditVcodeTransactionRequest>
    {
        public AddEditVcodeTransactionRequestValidator(IStringLocalizer<AddEditVcodeTransactionRequestValidator> localizer)
        {
            RuleFor(request => request.Reason).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Reason is required!"]);
            RuleFor(request => request.TransactionAmount).NotNull().NotEmpty().NotEqual(0).WithMessage(x => localizer["Transaction Amount is required!"]);
            When(request => request.Transactiontype == Enums.TransactionType.Debit && request.RequestId == null, () =>
             {
                 RuleFor(a => a.TransactionAmount).LessThanOrEqualTo(a => a.Credit).WithMessage(localizer["Debit Transaction Amount Must be Less Than Vote Code Credit"]);

             });
        }
    }
}
