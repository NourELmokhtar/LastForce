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
    public class SclateActionCommand : IRequest<IResult<int>>
    {
        public int ActionId { get; set; }
        public string Note { get; set; }
    }
    internal class SclateActionCommandHandler : IRequestHandler<SclateActionCommand, IResult<int>>
    {
        private readonly IMprRequestRepository _repository;

        public SclateActionCommandHandler(IMprRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResult<int>> Handle(SclateActionCommand request, CancellationToken cancellationToken)
        {
            return await Result<int>.SuccessAsync(await _repository.EsclateRequestAsync(request.ActionId, request.Note));
        }
    }
}
