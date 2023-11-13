using Forces.Application.Features.Items.Queries.GetAll;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.InventoryItem.Queries.GetAll
{
    public class GetAllInventoryItemQuery : IRequest<IResult<List<GetAllInventoryItemsResponse>>>
    {
        public GetAllInventoryItemQuery()
        {

        }
    }
    internal class GetAllInventoryItemQueryHandler : IRequestHandler<GetAllInventoryItemQuery, IResult<List<GetAllInventoryItemsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IVoteCodeService _voteCodeService;

        public GetAllInventoryItemQueryHandler(IUnitOfWork<int> unitOfWork, IVoteCodeService voteCodeService)
        {
            _unitOfWork = unitOfWork;
            _voteCodeService = voteCodeService;
        }

        public async Task<IResult<List<GetAllInventoryItemsResponse>>> Handle(GetAllInventoryItemQuery request, CancellationToken cancellationToken)
        {
            var ItemsList = await _unitOfWork.Repository<Models.InventoryItem>().Entities.Include(x => x.MeasureUnit).ToListAsync();
            var vCodes = await _voteCodeService.GetAllCodes();
            var MappedItems = (from item in ItemsList
                               
                               select new GetAllInventoryItemsResponse()
                               {
                                   ItemArName = item.ItemArName,
                                   Id = item.Id,
                                   ItemCode = item.ItemCode,
                                   ItemDescription = item.ItemDescription,
                                   ItemName = item.ItemName,
                                   ItemNsn = item.ItemNsn,
                                   MeasureUnitId = item.MeasureUnitId,
                                   MeasureName = item.MeasureUnit.Name,
                                   ItemClass = item.ItemClass,
                                   SerialNumber = item.SerialNumber,
                                   DateOfEnter = item.DateOfEnter,
                                   EndOfServiceDate = item.EndOfServiceDate,
                                   InventoryId = (int)item.InventoryId,
                                   FirstUseDate = item.FirstUseDate
                               }
                         ).ToList();

            //var MappedItems = ItemsList.Select(x => new GetAllItemsResponse()
            //{
            //    ItemArName = x.ItemArName,
            //    Id = x.Id,
            //    ItemCode = x.ItemCode,
            //    ItemDescription = x.ItemDescription,
            //    ItemName = x.ItemName,
            //    ItemNsn = x.ItemNsn,
            //    MeasureUnitId = x.MeasureUnitId,
            //    MeasureName = x.MeasureUnit.Name,
            //    VoteCodesId = x.VoteCodesId
            //}).ToList();
            return await Result<List<GetAllInventoryItemsResponse>>.SuccessAsync(MappedItems);
        }
    }
}
