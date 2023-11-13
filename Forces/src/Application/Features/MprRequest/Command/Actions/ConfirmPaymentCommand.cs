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
    public class ConfirmPaymentCommand : IRequest<IResult<int>>
    {
        public int RequestId { get; set; }
    }
    internal class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, IResult<int>>
    {
        private readonly IMprRequestRepository _repository;

        public ConfirmPaymentCommandHandler(IMprRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResult<int>> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            return await Result<int>.SuccessAsync(await _repository.ConfirmPaied(request.RequestId));
        }
    }
}
