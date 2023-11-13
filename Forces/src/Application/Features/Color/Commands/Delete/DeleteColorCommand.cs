using Forces.Application.Interfaces.Repositories;
using Forces.Domain.Entities.Catalog;
using Forces.Shared.Constants.Application;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Color.Commands.Delete
{
    public class DeleteColorCommand : IRequest<Result<int>>
    {
        public int ColorID { get; set; }
    }
    internal class DeleteColorCommandHandler : IRequestHandler<DeleteColorCommand, Result<int>>
    {

        private readonly IStringLocalizer<DeleteColorCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteColorCommandHandler(IStringLocalizer<DeleteColorCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {

            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeleteColorCommand command, CancellationToken cancellationToken)
        {

            var isColorUsed = await _unitOfWork.Repository<Models.Vehicle>().Entities.AnyAsync(x => x.ColorID == command.ColorID);
            if (!isColorUsed)
            {
                var color = await _unitOfWork.Repository<Models.Color>().GetByIdAsync(command.ColorID);
                if (color != null)
                {
                    await _unitOfWork.Repository<Models.Color>().DeleteAsync(color);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(color.Id, _localizer["Color Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Color Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["This Color Inuse,Deletion Not Allowed"]);
            }
        }
    }
}


