using Forces.Application.Enums;
using Forces.Application.Features.Documents.Commands.AddEdit;
using Forces.Application.Features.MprRequest.Dto.Request;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Application.Models;
using Forces.Application.Requests;
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

namespace Forces.Application.Features.MprRequest.Command
{
    public class AddMprRequestCommand:IRequest<IResult<int>>
    {
        [Required]
        public Priority Priority { get; set; } = Priority.Normal;
        [Required] 
        public int VoteCodeId { get; set; }
        public string RequestNote { get; set; }
        [Required]
        public List<ItemDto> RequestItems { get; set; }
        public List<UploadRequest> Attachments { get; set; }
    }
    internal class AddMprRequestCommandhandler : IRequestHandler<AddMprRequestCommand, IResult<int>>
    {
        private readonly IMprRequestRepository _repository;
        private readonly IStringLocalizer<AddMprRequestCommandhandler> _localizer;
        private readonly IUploadService _uploadService;
        
         

        public AddMprRequestCommandhandler(IMprRequestRepository repository, IStringLocalizer<AddMprRequestCommandhandler> localizer, IUploadService uploadService)
        {
            _repository = repository;
            _localizer = localizer;
            _uploadService = uploadService; 
        }

        public async Task<IResult<int>> Handle(AddMprRequestCommand request, CancellationToken cancellationToken)
        {
            var mprRequest = new Models.MprRequest();
            mprRequest.Priority = request.Priority;
            mprRequest.VoteCodeId = request.VoteCodeId;
            mprRequest.RequestNote = request.RequestNote;
            mprRequest.RequestState = RequestState.Pending;
            mprRequest.CurrentStep = MprSteps.CreationStep;

            mprRequest.RequestItems = new List<RequestItem>() ;
            foreach (var item in request.RequestItems)
            {
                mprRequest.RequestItems.Add(new RequestItem()
                {
                    ItemArName = item.ItemArName,
                    ItemClass = item.ItemClass,
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName,
                    ItemId = item.ItemId,
                    ItemNSN = item.ItemNSN,
                    ItemQty = item.ItemQty,
                    ItemPrice = item.ItemPrice,
                    Unit = item.Unit,
                });
            }
           await _repository.AddRequestAsync(mprRequest);
            // Adding Request Attachments if Avilable
            if (request.Attachments.Count>0)
            {
                var attachments = new List<MprRequestAttachments>();
                foreach (var attachment in request.Attachments)
                {
                    attachment.FileName = $"Q-{mprRequest.RequestRefranceCode.Replace("/", "-")}-{Guid.NewGuid()}{attachment.Extension}";
                    mprRequest.Attachments.Add(new MprRequestAttachments()
                    {
                        AttachmentType = "Qutation",
                        RequestID = mprRequest.Id,
                        FileUrl = _uploadService.UploadAsync(attachment),
                    });
                }
             var saved =   await _repository.AddAttachmentsAsync(mprRequest.Attachments.ToList(), mprRequest.Id);
            }

            // Adding First Creation Action
            MprRequestAction Creationaction = new MprRequestAction();
            Creationaction.RequestId = mprRequest.Id;
            Creationaction.ForceId = mprRequest.ForceId;
            Creationaction.ActionState = ActionState.Done;
            Creationaction.BaseId = mprRequest.BaseId;
            Creationaction.SectionId = mprRequest.SectionId;
            Creationaction.Readed = true;
            Creationaction.Step = MprSteps.CreationStep;
            Creationaction.Seen = true; 
            await _repository.AddActionAsync(Creationaction, mprRequest.Id, RequestState.Pending);
            // Adding Next Action
            MprRequestAction action = new MprRequestAction();
            action.RequestId = mprRequest.Id;
            action.ForceId = mprRequest.ForceId;
            action.ActionState = ActionState.Pending;
            action.BaseId = mprRequest.BaseId;
            action.SectionId = mprRequest.SectionId;
            action.Readed = false;
            action.Step = _repository.GetNextStep(mprRequest);
            action.Seen = false;
            await _repository.AddActionAsync(action, mprRequest.Id, RequestState.Pending);

            //notify Users
           await _repository.NotifyOwner(mprRequest);
           await _repository.NotifySteppers(mprRequest);
            return await Result<int>.SuccessAsync(mprRequest.Id, "Request Submited Succesfuly!");

        }
    }
}
