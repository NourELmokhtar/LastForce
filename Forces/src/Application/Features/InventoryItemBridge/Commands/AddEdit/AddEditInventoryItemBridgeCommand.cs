using AutoMapper;
using Forces.Application.Enums;
using Forces.Application.Features.InventoryInventoryItem.Commands.AddEdit;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.InventoryItemBridge.Commands.AddEdit
{
    public class AddEditInventoryItemBridgeCommand : IRequest<IResult<int>>
    {
        public int Id { get; set; }
        public int InventoryId {  get; set; }
        public int InventoryItemId {  get; set; }

        public string? SerialNumber { get; set; }
        public DateTime DateOfEnter { get; set; }

    }

    internal class AddEditInventoryItemBridgeCommandHandler : IRequestHandler<AddEditInventoryItemBridgeCommand, IResult<int>>
    {
        private protected IInventoryItemRepository _InventoryItemsRepository;
        private protected IUnitOfWork<int> _unitOfWork;
        private protected IMapper _mapper;
        private readonly IStringLocalizer<AddEditInventoryItemCommandHandler> _localizer;
        private readonly IVoteCodeService _voteCodeService;
        public AddEditInventoryItemBridgeCommandHandler(IInventoryItemRepository InventoryItemsRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditInventoryItemCommandHandler> localizer, IVoteCodeService voteCodeService)
        {
            _InventoryItemsRepository = InventoryItemsRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _voteCodeService = voteCodeService;
        }

        public async Task<IResult<int>> Handle(AddEditInventoryItemBridgeCommand request, CancellationToken cancellationToken)
        {
            
            if (request.Id == 0)
            {
                Models.InventoryItemBridge InventoryItem = new Models.InventoryItemBridge
                {
                    DateOfEnter = request.DateOfEnter,
                    InventoryId = request.InventoryId,
                    SerialNumber = request.SerialNumber,
                    InventoryItemId = request.InventoryItemId,
                };
                await _unitOfWork.Repository<Application.Models.InventoryItemBridge>().AddAsync(InventoryItem);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(InventoryItem.Id, _localizer["InventoryItem Added!"]);
            }
            else
            {
                Models.InventoryItemBridge InventoryItemBridge = new Models.InventoryItemBridge
                {
                    Id = request.Id,
                    DateOfEnter = request.DateOfEnter,
                    InventoryId = request.InventoryId,
                    SerialNumber = request.SerialNumber,
                    InventoryItemId= request.InventoryItemId,
                };


                var dbItem = await _unitOfWork.Repository<Models.InventoryItemBridge>().GetByIdAsync(InventoryItemBridge.Id);
                if (dbItem != null)
                {


                    await _unitOfWork.Repository<Models.InventoryItemBridge>().UpdateAsync(InventoryItemBridge);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(InventoryItemBridge.Id, _localizer["InventoryItemBridge Updated!"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["InventoryItemBridge Not Found!"]);
                }
            }
        }
    }


}
