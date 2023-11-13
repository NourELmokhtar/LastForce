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

namespace Forces.Application.Features.Person.Commands.Delete
{
    public class DeletePersonCommand : IRequest<IResult<int>>
    {
        [Required]
        public int Id { get; set; }
    }
    internal class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, IResult<int>>
    {
        private readonly IStringLocalizer<DeletePersonCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeletePersonCommandHandler(IStringLocalizer<DeletePersonCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<int>> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            var Person = await _unitOfWork.Repository<Application.Models.Person>().GetByIdAsync(request.Id);
            if (Person != null)
            {
                await _unitOfWork.Repository<Application.Models.Person>().DeleteAsync(Person);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(Person.Id, _localizer["Person Deleted"]);

            }
            return await Result<int>.FailAsync(_localizer["Person Not Found!"]);

        }
    }
}
