using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.AirCraft.Commands.Delete
{
    public class DeleteAirCraftCommand : IRequest<Result<int>>
    {
        [Required]
        public int Id { get; set; }
    }
    internal class DeleteAirCraftCommandHandler : IRequestHandler<DeleteAirCraftCommand, Result<int>>
    {
        private readonly IStringLocalizer<DeleteAirCraftCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IAirCraftRepository _repository;

        public DeleteAirCraftCommandHandler(IStringLocalizer<DeleteAirCraftCommandHandler> localizer, IUnitOfWork<int> unitOfWork, IAirCraftRepository repository)
        {
            _localizer = localizer;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<Result<int>> Handle(DeleteAirCraftCommand command, CancellationToken cancellationToken)
        {
            var isUsed = await _repository.IsAirCraftInused(command.Id);
            if (isUsed)
            {
                return await Result<int>.FailAsync("This Air Craft is Inuse , Can not Deleted !");
            }
            else
            {
                var AirCraft = await _unitOfWork.Repository<Models.AirCraft>().GetByIdAsync(command.Id);
                if (AirCraft != null)
                {
                  await  _unitOfWork.Repository<Models.AirCraft>().DeleteAsync(AirCraft);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(command.Id, "Air Craft Deleted!");

                }
                else
                {
                    return await Result<int>.FailAsync("Air Craft with this Id Not Found!");
                }
            }
             
        }
    }
}
