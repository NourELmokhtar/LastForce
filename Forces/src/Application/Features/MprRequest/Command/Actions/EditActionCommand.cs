using Forces.Application.Features.MprRequest.Dto.Request;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Application.Models;
using Forces.Application.Requests;
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
    public class EditActionCommand : IRequest<IResult<int>>
    {
        public List<ItemDto> Items { get; set; }
        public int ActionID { get; set; }
        public string Note { get; set; }
        public List<UploadRequest> Attachments { get; set; }
    }
    internal class EditActionCommandHandler : IRequestHandler<EditActionCommand, IResult<int>>
    {
        private readonly IMprRequestRepository _repository;
       

        public EditActionCommandHandler(IMprRequestRepository repository)
        {
            _repository = repository; 
        }

        public async Task<IResult<int>> Handle(EditActionCommand request, CancellationToken cancellationToken)
        {
            return await Result<int>.SuccessAsync(await _repository.EditRequestAsync(request.Items, request.ActionID, request.Note, request.Attachments));
        }
    }
}
