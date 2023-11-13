using Forces.Application.Features.InventoryItem.Queries.GetAll;
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

namespace Forces.Application.Features.InventoryItemBridge.Queries.GetAll
{
    public class GetAllInventoryItemBridgeQuery : IRequest<IResult<List<GetAllInventoryItemBridgeResponse>>>
    {
        public GetAllInventoryItemBridgeQuery() 
        {

        }
    }

    internal class GetAllInventoryItemQueryHandler : IRequestHandler<GetAllInventoryItemBridgeQuery, IResult<List<GetAllInventoryItemBridgeResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IVoteCodeService _voteCodeService;

        public GetAllInventoryItemQueryHandler(IUnitOfWork<int> unitOfWork, IVoteCodeService voteCodeService)
        {
            _unitOfWork = unitOfWork;
            _voteCodeService = voteCodeService;
        }

        public async Task<IResult<List<GetAllInventoryItemBridgeResponse>>> Handle(GetAllInventoryItemBridgeQuery request, CancellationToken cancellationToken)
        {
            var ItemsList = await _unitOfWork.Repository<Models.InventoryItemBridge>().Entities.Include(x => x.Inventory)
                .Include(x=>x.InventoryItem).ToListAsync();
            var MappedItems = (from item in ItemsList

                               select new GetAllInventoryItemBridgeResponse()
                               {
                                   SerialNumber = item.SerialNumber,
                                   InventoryName = _unitOfWork.Repository<Models.Inventory>().GetAllAsync().Result.Where(y=>y.Id==item.InventoryId).FirstOrDefault().Name,//item.InventoryItem.ItemName,
                                   InventoryItemName= _unitOfWork.Repository<Models.InventoryItem>().GetAllAsync().Result.Where(y => y.Id == item.InventoryItemId).FirstOrDefault().ItemName,
                                   DateOfEnter = item.DateOfEnter,

                               }
                         ).ToList();

            return await Result<List<GetAllInventoryItemBridgeResponse>>.SuccessAsync(MappedItems);
        }
    }

}
