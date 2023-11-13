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
    public class SubmitPaymentCommand : IRequest<IResult<int>>
    {
        public int RequestId { get; set; }
    }
    internal class SubmitPaymentCommandHandler : IRequestHandler<SubmitPaymentCommand, IResult<int>>
    {
        private readonly IMprRequestRepository _repository;

        public SubmitPaymentCommandHandler(IMprRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResult<int>> Handle(SubmitPaymentCommand request, CancellationToken cancellationToken)
        {
            return await Result<int>.SuccessAsync(await _repository.SubmitPayment(request.RequestId));
        }
    }
}
