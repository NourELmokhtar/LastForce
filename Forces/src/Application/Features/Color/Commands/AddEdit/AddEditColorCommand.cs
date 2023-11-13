using AutoMapper;
using Forces.Application.Features.Color.Commands.AddEdit;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Forces.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Forces.Application.Features.Color.Commands.AddEdit
{
    public class AddEditColorCommand : IRequest<Result<int>>

    {
        public int ColorID { get; set; }

        public string ColorName { get; set; }

    }
    internal class AddEditColrCommandHandler : IRequestHandler<AddEditColorCommand, Result<int>>
    {

        private readonly IStringLocalizer<AddEditColrCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditColrCommandHandler(IStringLocalizer<AddEditColrCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {

            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(AddEditColorCommand request, CancellationToken cancellationToken)
        {
            if (request.ColorID == 0)
            {
                var ExistColor = await _unitOfWork.Repository<Models.Color>().Entities.FirstOrDefaultAsync(x => x.ColorName == request.ColorName);
                if (ExistColor != null)
                {
                    return await Result<int>.FailAsync(_localizer["This Color Is Already Exist!"]);
                }
                else
                {
                    Models.Color color = new Models.Color()
                    {
                        ColorName = request.ColorName
                    };
                    await _unitOfWork.Repository<Models.Color>().AddAsync(color);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(color.Id, _localizer["Color Added Successfuly!"]);
                }
            }
            else
            {
                var ExistColor = await _unitOfWork.Repository<Models.Color>().Entities.FirstOrDefaultAsync(x => x.Id == request.ColorID);
                if (ExistColor == null)
                {
                    return await Result<int>.FailAsync(_localizer["Color Not Found!!"]);
                }
                else
                {
                    var ExistnameColor = await _unitOfWork.Repository<Models.Color>().Entities.FirstOrDefaultAsync(x => x.ColorName == request.ColorName && x.Id != request.ColorID);
                    if (ExistnameColor != null)
                    {
                        return await Result<int>.FailAsync(_localizer["This Color Is Already Exist!"]);
                    }
                    else
                    {
                        ExistColor.ColorName = request.ColorName;
                        await _unitOfWork.Repository<Models.Color>().UpdateAsync(ExistColor);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(ExistColor.Id, _localizer["Color Updated Successfuly!"]);
                    }
                }
            }
        }
    }
}

