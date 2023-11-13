using FluentValidation;
using Forces.Application.Requests.Requests.NPRRequest;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Validators.Requests.Requests.NPRRequest
{
    public class AddEditNPRValidator : AbstractValidator<AddEditNPRRequest>
    {
        public AddEditNPRValidator(IStringLocalizer<AddEditNPRValidator> localizer)
        {
            //  RuleFor(request => request.Attachments).Must(x => x.Count == 3).WithMessage(localizer["You Must Provide 3 Attachment At Least"]);
            RuleFor(x => x.ItemId).NotNull().NotEqual(0).WithMessage(localizer["You Must Supply an Item !"]);
            RuleFor(x => x.QTY).Must(x => x > 0).WithMessage(localizer["QTY Must Be More Than 0"]);

        }
    }
}
