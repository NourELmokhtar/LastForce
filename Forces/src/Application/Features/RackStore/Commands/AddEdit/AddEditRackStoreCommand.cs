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

namespace Forces.Application.Features.RackStore.Commands.AddEdit
{
    public class AddEditRackStoreCommand : IRequest<Result<int>>

    {
        public int RackCode { get; set; }
        public string RackName { get; set; }

    }
    internal class AddEditRackStoreCommandHandler : IRequestHandler<AddEditRackStoreCommand, Result<int>>
    {

        private readonly IStringLocalizer<AddEditRackStoreCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditRackStoreCommandHandler(IStringLocalizer<AddEditRackStoreCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {

            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(AddEditRackStoreCommand request, CancellationToken cancellationToken)
        {
            if (request.RackCode == 0)
            {
                var ExistRackStore = await _unitOfWork.Repository<Models.RackStore>().Entities.FirstOrDefaultAsync(x => x.RackName == request.RackName);
                if (ExistRackStore != null)
                {
                    return await Result<int>.FailAsync(_localizer["This Rack Is Already Exist!"]);
                }
                else

                {
                    Models.RackStore rack = new Models.RackStore()
                    {
                        RackName = request.RackName
                    };
                    await _unitOfWork.Repository<Models.RackStore>().AddAsync(rack);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(rack.Id, _localizer["Rack Added Successfuly!"]);


                }
            }
            else
            {
                var ExistRackStore = await _unitOfWork.Repository<Models.RackStore>().Entities.FirstOrDefaultAsync(x => x.Id == request.RackCode);
                if (ExistRackStore == null)
                {
                    return await Result<int>.FailAsync(_localizer["Rack Not Found!!"]);
                }
                else
                {
                    var ExistnameRack = await _unitOfWork.Repository<Models.RackStore>().Entities.FirstOrDefaultAsync(x => x.RackName == request.RackName && x.Id != request.RackCode);
                    if (ExistnameRack != null)
                    {
                        return await Result<int>.FailAsync(_localizer["This Rack Is Already Exist!"]);
                    }
                    else
                    {
                        ExistRackStore.RackName = request.RackName;
                        await _unitOfWork.Repository<Models.RackStore>().UpdateAsync(ExistRackStore);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(ExistRackStore.Id, _localizer["Color Updated Successfuly!"]);
                    }
                }
            }
        }
    }
}