using AutoMapper;
using Forces.Application.Enums;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Inventory.Commands.AddEdit
{
    public class AddEditInventoryCommand : IRequest<IResult<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? BaseSectionId { get; set; }
        public int? HouseId { get; set; }
        public int? RoomId { get; set; }
    }
    internal class AddEditInventoryCommandHandler : IRequestHandler<AddEditInventoryCommand, IResult<int>>
    {
        private protected IItemRepository _ItemsRepository;
        private protected IUnitOfWork<int> _unitOfWork;
        private protected IMapper _mapper;
        private readonly IStringLocalizer<AddEditInventoryCommandHandler> _localizer;
        private readonly IVoteCodeService _voteCodeService;
        public AddEditInventoryCommandHandler(IItemRepository itemsRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditInventoryCommandHandler> localizer, IVoteCodeService voteCodeService)
        {
            _ItemsRepository = itemsRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _voteCodeService = voteCodeService;
        }

        public async Task<IResult<int>> Handle(AddEditInventoryCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                var ExistInventory = await _unitOfWork.Repository<Models.Inventory>().Entities.FirstOrDefaultAsync(
                    (x => (x.Name == request.Name && x.BaseSectionId == request.BaseSectionId)
                    || (x.Name == request.Name && x.HouseId == request.HouseId)
                    || (x.Name == request.Name && x.RoomId == request.RoomId))
                    );
                if (ExistInventory != null)
                {
                    return await Result<int>.FailAsync(_localizer["This Inventory Name Is Already Exist!"]);
                }
                else
                {
                    Models.Inventory Inventory = new Models.Inventory()
                    {
                        Id = request.Id,
                        Name = request.Name,
                        RoomId= request.RoomId,
                        HouseId= request.HouseId,
                        BaseSectionId= request.BaseSectionId,
                    };
                    await _unitOfWork.Repository<Models.Inventory>().AddAsync(Inventory);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(Inventory.Id, _localizer["Inventory Added Successfuly!"]);
                }
            }
            else
            {
                var ExistInventory = await _unitOfWork.Repository<Models.Inventory>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (ExistInventory == null)
                {
                    return await Result<int>.FailAsync(_localizer["Inventory Not Found!!"]);
                }
                else
                {
                    var ExistnameOffice = await _unitOfWork.Repository<Models.Inventory>().Entities.FirstOrDefaultAsync(x => x.Name == request.Name && x.Id != request.Id);
                    if (ExistnameOffice != null)
                    {
                        return await Result<int>.FailAsync(_localizer["This Inventory Is Already Exist!"]);
                    }
                    else
                    {
                        ExistInventory.Name = request.Name;
                        await _unitOfWork.Repository<Models.Inventory>().UpdateAsync(ExistInventory);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(ExistInventory.Id, _localizer["Office Updated Successfuly!"]);
                    }
                }
            }
        }
    }
}
