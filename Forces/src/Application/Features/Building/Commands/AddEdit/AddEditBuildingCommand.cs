using AutoMapper;
using Forces.Application.Features.Building.Commands.AddEdit;
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

namespace Forces.Application.Features.Building.Commands.AddEdit
{
    public class AddEditBuildingCommand : IRequest<IResult<int>>
    {
        public int Id { get; set; }
        public string BuildingName { get; set; }
        public string BuildingCode { get; set; }
        public int BaseId { get; set; }
    }
    internal class AddEditBuildingCommandHandler : IRequestHandler<AddEditBuildingCommand, IResult<int>>
    {
        private protected IItemRepository _ItemsRepository;
        private protected IUnitOfWork<int> _unitOfWork;
        private protected IMapper _mapper;
        private readonly IStringLocalizer<AddEditBuildingCommandHandler> _localizer;
        private readonly IVoteCodeService _voteCodeService;
        public AddEditBuildingCommandHandler(IItemRepository itemsRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditBuildingCommandHandler> localizer, IVoteCodeService voteCodeService)
        {
            _ItemsRepository = itemsRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _voteCodeService = voteCodeService;
        }

        public async Task<IResult<int>> Handle(AddEditBuildingCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                var ExistBuilding = await _unitOfWork.Repository<Models.Building>().Entities.FirstOrDefaultAsync(
                    (x => (x.BuildingName == request.BuildingName && x.BaseId == request.BaseId)));
                    
                if (ExistBuilding != null)
                {
                    return await Result<int>.FailAsync(_localizer["This Building Name Is Already Exist!"]);
                }
                else
                {
                    Models.Building Building = new Models.Building()
                    {
                        Id = request.Id,
                        BuildingName = request.BuildingName,
                        BaseId = request.BaseId,
                        BuildingCode = request.BuildingCode,
                    };
                    await _unitOfWork.Repository<Models.Building>().AddAsync(Building);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(Building.Id, _localizer["Building Added Successfuly!"]);
                }
            }
            else
            {
                var ExistBuilding = await _unitOfWork.Repository<Models.Building>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (ExistBuilding == null)
                {
                    return await Result<int>.FailAsync(_localizer["Building Not Found!!"]);
                }
                else
                {
                    var ExistnameOffice = await _unitOfWork.Repository<Models.Building>().Entities.FirstOrDefaultAsync(x => x.BuildingName == request.BuildingName && x.Id != request.Id);
                    if (ExistnameOffice != null)
                    {
                        return await Result<int>.FailAsync(_localizer["This Building Is Already Exist!"]);
                    }
                    else
                    {
                        ExistBuilding.BuildingName = request.BuildingName;
                        await _unitOfWork.Repository<Models.Building>().UpdateAsync(ExistBuilding);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(ExistBuilding.Id, _localizer["Building Updated Successfuly!"]);
                    }
                }
            }
        }
    }
}
