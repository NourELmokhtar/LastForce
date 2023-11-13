using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.MprRequest.Command.Actions
{
    public class RejectActionCommand : IRequest<IResult<int>>
    {
        public int ActionId { get; set; }
        public string Note { get; set; }
    }
    internal class RejectActionCommandHandler : IRequestHandler<RejectActionCommand, IResult<int>>
    {
        private readonly IMprRequestRepository _repository;

        public RejectActionCommandHandler(IMprRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResult<int>> Handle(RejectActionCommand request, CancellationToken cancellationToken)
        {
            return await Result<int>.SuccessAsync(await _repository.RejectRequestAsync(request.ActionId, request.Note));
        }
    }
}
