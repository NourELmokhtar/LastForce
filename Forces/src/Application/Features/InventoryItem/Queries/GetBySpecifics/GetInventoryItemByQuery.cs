using Forces.Application.Extensions;
using Forces.Application.Features.Items.Queries.GetBySpecifics;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.InventoryItem.Queries.GetBySpecifics
{
    public class GetInventoryItemByQuery : IRequest<IResult<List<GetInventoryItemByResponse>>>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string NSN { get; set; }
        public string Code { get; set; }
        public int? MeasureID { get; set; }

    }
    internal class GetAllItemsQuesryHandler : IRequestHandler<GetInventoryItemByQuery, IResult<List<GetInventoryItemByResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllItemsQuesryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<List<GetInventoryItemByResponse>>> Handle(GetInventoryItemByQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Models.InventoryItem, bool>> Condition = x => x.Id != 0;
            if (request.Id.HasValue)
            {
                Condition = Condition.And(x => x.Id == request.Id.Value);
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                Condition = Condition.And(x => x.ItemName.Contains(request.Name) || x.ItemArName.Contains(request.Name));
            }
            if (!string.IsNullOrEmpty(request.NSN))
            {
                Condition = Condition.And(x => x.ItemNsn.StartsWith(request.NSN));
            }
            if (!string.IsNullOrEmpty(request.Code))
            {
                Condition = Condition.And(x => x.ItemCode.StartsWith(request.Code));
            }
            if (request.MeasureID.HasValue)
            {
                Condition = Condition.And(x => x.MeasureUnitId == request.MeasureID.Value);
            }
            var ItemsList = await _unitOfWork.Repository<Models.InventoryItem>().Entities.Include(x => x.MeasureUnit).Where(Condition).ToListAsync();
            var MappedItemsList = ItemsList.Select(x => new GetInventoryItemByResponse()
            {
                ItemArName = x.ItemArName,
                Id = x.Id,
                ItemCode = x.ItemCode,
                ItemDescription = x.ItemDescription,
                ItemName = x.ItemName,
                ItemNsn = x.ItemNsn,
                MeasureUnitId = x.MeasureUnitId,
                MeasureName = x.MeasureUnit.Name
            }).ToList();
            return await Result<List<GetInventoryItemByResponse>>.SuccessAsync(MappedItemsList);
        }
    }
}
