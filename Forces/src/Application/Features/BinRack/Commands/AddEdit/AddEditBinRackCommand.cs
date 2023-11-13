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

namespace Forces.Application.Features.BinRack.Commands.AddEdit
{
    public class AddEditBinRackCommand : IRequest<Result<int>>
    {
        public int BinCode { get; set; }
        public string BinName { get; set; }
    }
    internal class AddEditBinRackCommandHandler : IRequestHandler<AddEditBinRackCommand, Result<int>>
    {

        private readonly IStringLocalizer<AddEditBinRackCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditBinRackCommandHandler(IStringLocalizer<AddEditBinRackCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {

            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(AddEditBinRackCommand request, CancellationToken cancellationToken)
        {
            if (request.BinCode == 0)
            {
                var ExistBinCode = await _unitOfWork.Repository<Models.BinRack>().Entities.FirstOrDefaultAsync(x => x.BinName == request.BinName);
                if (ExistBinCode != null)
                {
                    return await Result<int>.FailAsync(_localizer["This Color Is Already Exist!"]);
                }
                else
                {
                    Models.BinRack binRack = new Models.BinRack()
                    {
                        BinName = request.BinName
                    };
                    await _unitOfWork.Repository<Models.BinRack>().AddAsync(binRack);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(binRack.Id, _localizer["BinRack Added Successfuly!"]);
                }
            }
            else
            {
                var ExistBinRack = await _unitOfWork.Repository<Models.BinRack>().Entities.FirstOrDefaultAsync(x => x.BinCode == request.BinCode);
                if (ExistBinRack == null)
                {
                    return await Result<int>.FailAsync(_localizer["Bin Not Found!!"]);
                }
                else
                {
                    var ExistnameBin = await _unitOfWork.Repository<Models.BinRack>().Entities.FirstOrDefaultAsync(x => x.BinName == request.BinName && x.Id != request.BinCode);
                    if (ExistnameBin != null)
                    {
                        return await Result<int>.FailAsync(_localizer["This Color Is Already Exist!"]);
                    }
                    else
                    {
                        ExistBinRack.BinName = request.BinName;
                        await _unitOfWork.Repository<Models.BinRack>().UpdateAsync(ExistBinRack);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(ExistBinRack.BinCode, _localizer["Bin Updated Successfuly!"]);
                    }

                }
            }
        }
    }
}


