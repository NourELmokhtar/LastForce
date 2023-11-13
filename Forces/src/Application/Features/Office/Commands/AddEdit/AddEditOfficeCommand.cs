using AutoMapper;
using Forces.Application.Features.Office.Commands.AddEdit;
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

namespace Forces.Application.Features.Office.Commands.AddEdit
{
    public class AddEditOfficeCommand : IRequest<IResult<int>>
    {
        public int Id { get; set; }
        public string OfficeName { get; set; }
        public string OfficeCode { get; set; }
        public int BaseSectionId { get; set; }
    }

    internal class AddEditOfficeCommandHandler : IRequestHandler<AddEditOfficeCommand, IResult<int>>
    {
        private protected IItemRepository _ItemsRepository;
        private protected IUnitOfWork<int> _unitOfWork;
        private protected IMapper _mapper;
        private readonly IStringLocalizer<AddEditOfficeCommandHandler> _localizer;
        private readonly IVoteCodeService _voteCodeService;
        public AddEditOfficeCommandHandler(IItemRepository itemsRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditOfficeCommandHandler> localizer, IVoteCodeService voteCodeService)
        {
            _ItemsRepository = itemsRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _voteCodeService = voteCodeService;
        }

        public async Task<IResult<int>> Handle(AddEditOfficeCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                var ExistOffice = await _unitOfWork.Repository<Models.Office>().Entities.FirstOrDefaultAsync(
                    (x => (x.Name == request.OfficeName && x.BasesSectionsId == request.BaseSectionId)));

                if (ExistOffice != null)
                {
                    return await Result<int>.FailAsync(_localizer["This Office Name Is Already Exist!"]);
                }
                else
                {
                    Models.Office Office = new Models.Office()
                    {
                        Id = request.Id,
                        Name = request.OfficeName,
                        BasesSectionsId = request.BaseSectionId,
                        
                    };
                    await _unitOfWork.Repository<Models.Office>().AddAsync(Office);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(Office.Id, _localizer["Office Added Successfuly!"]);
                }
            }
            else
            {
                var ExistOffice = await _unitOfWork.Repository<Models.Office>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (ExistOffice == null)
                {
                    return await Result<int>.FailAsync(_localizer["Office Not Found!!"]);
                }
                else
                {
                    var ExistnameOffice = await _unitOfWork.Repository<Models.Office>().Entities.FirstOrDefaultAsync(x => x.Name == request.OfficeName && x.Id != request.Id);
                    if (ExistnameOffice != null)
                    {
                        return await Result<int>.FailAsync(_localizer["This Office Is Already Exist!"]);
                    }
                    else
                    {
                        ExistOffice.Name = request.OfficeName;
                        await _unitOfWork.Repository<Models.Office>().UpdateAsync(ExistOffice);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(ExistOffice.Id, _localizer["Office Updated Successfuly!"]);
                    }
                }
            }
        }
    }
}
