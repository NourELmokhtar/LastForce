using Forces.Application.Enums;
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
    public class RedirectActionCommand :IRequest<IResult<int>>
    {
        public int ActionId { get; set; }
        public RedirectAction To { get; set; }
        public string RefId { get; set; }
        public string Note { get; set; }
        public int? RefIdInt { get; set; }

    }
    internal class RedirectActionCommandHandler : IRequestHandler<RedirectActionCommand, IResult<int>>
    {
        private readonly IMprRequestRepository _repository;

        public RedirectActionCommandHandler(IMprRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResult<int>> Handle(RedirectActionCommand request, CancellationToken cancellationToken)
        {
            return await Result<int>.SuccessAsync(await _repository.RedirectRequestAsync(request.ActionId, request.To,request.RefId,request.Note,request.RefIdInt));
        }
    }
}
