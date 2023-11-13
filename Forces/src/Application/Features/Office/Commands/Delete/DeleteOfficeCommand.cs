using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Forces.Application.Features.Office.Commands.Delete
{
    public class DeleteOfficeCommand : IRequest<IResult<int>>
    {
        [Required]
        public int OfficeId { get; set; }
    }
    internal class DeleteOfficeCommandHandler : IRequestHandler<DeleteOfficeCommand, IResult<int>>
    {
        private readonly IStringLocalizer<DeleteOfficeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteOfficeCommandHandler(IStringLocalizer<DeleteOfficeCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<int>> Handle(DeleteOfficeCommand request, CancellationToken cancellationToken)
        {
            var Office = await _unitOfWork.Repository<Models.Office>().GetByIdAsync(request.OfficeId);
            if (Office != null)
            {
                await _unitOfWork.Repository<Models.Office>().DeleteAsync(Office);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(Office.Id, _localizer["Office Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Office Not Found!"]);
            }
        }
    }
}
