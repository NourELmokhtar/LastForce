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
    public class SelectQutationActionCommand : IRequest<IResult<int>>
    {
        public int ActionId { get; set; }
        public string Note { get; set; }
        public List<string> SelectedAttachments { get; set; }
    }
    internal class SelectQutationActionCommandHandler : IRequestHandler<SelectQutationActionCommand, IResult<int>>
    {
        private readonly IMprRequestRepository _repository;

        public SelectQutationActionCommandHandler(IMprRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResult<int>> Handle(SelectQutationActionCommand request, CancellationToken cancellationToken)
        {
            return await Result<int>.SuccessAsync(await _repository.SelectAttachmentAsync(request.ActionId, request.Note, request.SelectedAttachments));
        }
    }
}
