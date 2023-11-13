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
    public class SubmitActionCommand : IRequest<IResult<int>>
    {
        public int ActionId { get; set; }
        public string Note { get; set; }
        public int? VodeCodeId { get; set; }
    }
    internal class SubmitActionCommandHandler : IRequestHandler<SubmitActionCommand, IResult<int>>
    {
        private readonly IMprRequestRepository _repository;

        public SubmitActionCommandHandler(IMprRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResult<int>> Handle(SubmitActionCommand request, CancellationToken cancellationToken)
        {
            return await Result<int>.SuccessAsync(await _repository.SubmitRequestAsync(request.ActionId, request.Note,request.VodeCodeId));
        }
    }
}
