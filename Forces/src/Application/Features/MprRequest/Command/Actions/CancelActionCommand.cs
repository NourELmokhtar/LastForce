using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Application.Models;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.MprRequest.Command.Actions
{
    public class CancelActionCommand : IRequest<IResult<int>>
    {
        public int RequestId { get; set; }
        public string Reason { get; set; }
    }
    internal class CancelActionCommandHandler : IRequestHandler<CancelActionCommand, IResult<int>>
    {
        private readonly IMprRequestRepository _repository; 

        public CancelActionCommandHandler(IMprRequestRepository repository )
        {
            _repository = repository; 
        }

        public async Task<IResult<int>> Handle(CancelActionCommand request, CancellationToken cancellationToken)
        {
            return await Result<int>.SuccessAsync(await _repository.CancelRequestAsync(request.RequestId, request.Reason));
        }
    }

}
