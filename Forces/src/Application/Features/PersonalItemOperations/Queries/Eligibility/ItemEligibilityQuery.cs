using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.PersonalItemOperations.Queries.Eligibility
{
    public class ItemEligibilityQuery : IRequest<IResult<EligibilityModel>>
    {
        public int PersonalItemId { get; set; }
        public string UserId { get; set; }
        public int Qty { get; set; }

    }
    internal class ItemEligibilityQueryHandler : IRequestHandler<ItemEligibilityQuery, IResult<EligibilityModel>>
    {
        private readonly IStringLocalizer<ItemEligibilityQueryHandler> _localizer;
        private protected IUnitOfWork<int> _unitOfWork;

        public ItemEligibilityQueryHandler(IStringLocalizer<ItemEligibilityQueryHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<EligibilityModel>> Handle(ItemEligibilityQuery request, CancellationToken cancellationToken)
        {
            var personalItem = await _unitOfWork.Repository<Models.PersonalItems>().GetByIdAsync(request.PersonalItemId);
            var UserOperations = await _unitOfWork.Repository<Models.PersonalItemsOperation_Details>().Entities.Where(x => x.UserId == request.UserId && x.PersonalItemId == request.PersonalItemId).ToListAsync();
            if (UserOperations.Count == 0)
            {
                if (request.Qty > personalItem.MaxQtyOnPeriod)
                {
                    return await Result<EligibilityModel>.FailAsync(_localizer["Maxmum Quntity Of This Item On This Period is {0}!", personalItem.MaxQtyOnPeriod]);
                }
                return await Result<EligibilityModel>.SuccessAsync(new EligibilityModel(personalItem.Id, request.UserId, true, personalItem.MaxQtyOnPeriod.Value), _localizer["Item Added!"]);
            }
            DateTime MaxPeriod = DateTime.Now;
            switch (personalItem.UsagePeriodUnit)
            {
                case Enums.UsagePeriodUnit.Day:
                    MaxPeriod = MaxPeriod.AddDays((personalItem.UsagePeriod.Value * -1));
                    break;
                case Enums.UsagePeriodUnit.Month:
                    MaxPeriod = MaxPeriod.AddMonths((personalItem.UsagePeriod.Value * -1));
                    break;
                case Enums.UsagePeriodUnit.Year:
                    MaxPeriod = MaxPeriod.AddYears((personalItem.UsagePeriod.Value * -1));
                    break;
                default:
                    MaxPeriod = DateTime.Now;
                    break;
            }
            var TotalQty = UserOperations.Where(x => x.OperationDate >= MaxPeriod && x.OperationDate <= DateTime.Now).Sum(x => x.Qty);
            if (TotalQty >= personalItem.MaxQtyOnPeriod)
            {
                return await Result<EligibilityModel>.FailAsync(_localizer["Maxmum Quntity Of This Item On This Period is {0}!", personalItem.MaxQtyOnPeriod - TotalQty]);
            }
            return await Result<EligibilityModel>.SuccessAsync(new EligibilityModel(personalItem.Id, request.UserId, true, personalItem.MaxQtyOnPeriod.Value - TotalQty), _localizer["Item Added!"]);


        }
    }
}
