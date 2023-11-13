using AutoMapper;
using Forces.Application.Features.House.Commands.AddEdit;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
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

namespace Forces.Application.Features.House.Commands.AddEdit
{
    public class AddEditHouseCommand : IRequest<IResult<int>>
    {
        public int Id { get; set; }
        public string HouseName { get; set; }
        public string HouseCode { get; set; }
        public int BaseId { get; set; }
    }

    internal class AddEditHouseCommandHandler : IRequestHandler<AddEditHouseCommand, IResult<int>>
    {
        private protected IItemRepository _ItemsRepository;
        private protected IUnitOfWork<int> _unitOfWork;
        private protected IMapper _mapper;
        private readonly IStringLocalizer<AddEditHouseCommandHandler> _localizer;
        private readonly IVoteCodeService _voteCodeService;
        public AddEditHouseCommandHandler(IItemRepository itemsRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditHouseCommandHandler> localizer, IVoteCodeService voteCodeService)
        {
            _ItemsRepository = itemsRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _voteCodeService = voteCodeService;
        }

        public async Task<IResult<int>> Handle(AddEditHouseCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                var ExistHouse = await _unitOfWork.Repository<Models.House>().Entities.FirstOrDefaultAsync(
                    (x => (x.HouseName == request.HouseName && x.BaseId == request.BaseId)));

                if (ExistHouse != null)
                {
                    return await Result<int>.FailAsync(_localizer["This House Name Is Already Exist!"]);
                }
                else
                {
                    Models.House House = new Models.House()
                    {
                        Id = request.Id,
                        HouseName = request.HouseName,
                        BaseId = request.BaseId,
                        HouseCode = request.HouseCode,
                    };
                    await _unitOfWork.Repository<Models.House>().AddAsync(House);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(House.Id, _localizer["House Added Successfuly!"]);
                }
            }
            else
            {
                var ExistHouse = await _unitOfWork.Repository<Models.House>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (ExistHouse == null)
                {
                    return await Result<int>.FailAsync(_localizer["House Not Found!!"]);
                }
                else
                {
                    var ExistnameOffice = await _unitOfWork.Repository<Models.House>().Entities.FirstOrDefaultAsync(x => x.HouseName == request.HouseName && x.Id != request.Id);
                    if (ExistnameOffice != null)
                    {
                        return await Result<int>.FailAsync(_localizer["This House Is Already Exist!"]);
                    }
                    else
                    {
                        ExistHouse.HouseName = request.HouseName;
                        await _unitOfWork.Repository<Models.House>().UpdateAsync(ExistHouse);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(ExistHouse.Id, _localizer["House Updated Successfuly!"]);
                    }
                }
            }
        }
    }
}
