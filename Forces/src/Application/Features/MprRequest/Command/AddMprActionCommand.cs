using AutoMapper.Internal;
using Forces.Application.Enums;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Application.Models;
using Forces.Application.Requests;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.MprRequest.Command
{
    public class AddMprActionCommand : IRequest<IResult<int>>
    {
        public int RequestId { get; set; }
        public StepActions Action { get; set; }
        public int? refId { get; set; }
        public string refIdStr { get; set; }
        public List<UploadRequest> Attachments { get; set; }
        public string ActionNote { get; set; }
        public RedirectAction? RedirectTo { get; set; }
        public int VoteCodeId { get; set; }
    }
    internal class AddMprActionCommandHandler : IRequestHandler<AddMprActionCommand, IResult<int>>
    {
        private readonly IMprRequestRepository _repository;
        private readonly IStringLocalizer<AddMprRequestCommandhandler> _localizer;
        private readonly IUploadService _uploadService;

        public AddMprActionCommandHandler(IMprRequestRepository repository, IStringLocalizer<AddMprRequestCommandhandler> localizer, IUploadService uploadService)
        {
            _repository = repository;
            _localizer = localizer;
            _uploadService = uploadService;
        }

        public async Task<IResult<int>> Handle(AddMprActionCommand request, CancellationToken cancellationToken)
        {
            var mpr = await _repository.GetRequestByIdAsync(request.RequestId);
            MprRequestAction action = new MprRequestAction();
            switch (request.Action)
            {
                case StepActions.Submit:
                    action.Step = MprSteps.VoteCodeContreoller;
                   await _repository.AddActionAsync(action, mpr.Id, RequestState.Pending);
                    break;
                case StepActions.Esclate:
                   mpr.CurrentStep = _repository.GetNextStep(mpr);
                    break;
                case StepActions.Reject:
                    mpr.RequestState = RequestState.Rejected;
                    action.ActionsType = StepActions.Reject;
                    await _repository.AddActionAsync(action, mpr.Id, RequestState.Pending);
                    break;
                case StepActions.Redirect:
                    if (request.RedirectTo.HasValue)
                    {
                        switch (request.RedirectTo.Value)
                        {
                            case RedirectAction.ToUser:
                                action.TargetUserId = request.refIdStr;
                                await _repository.NotifyStepperUser(mpr, request.refIdStr);
                                break;
                            case RedirectAction.ToUserType:
                                break;
                            case RedirectAction.ToDepartment:
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case StepActions.Edit:

                    if (request.Attachments.Count >0)
                    {
                        
                            var attachments = new List<MprRequestAttachments>();
                            foreach (var attachment in request.Attachments)
                            {
                                attachment.FileName = $"Q-{mpr.RequestRefranceCode.Replace("/", "-")}-{Guid.NewGuid()}{attachment.Extension}";
                                action.Attachments.Add(new MprRequestAttachments()
                                {
                                    AttachmentType = "Qutation",
                                    ActionId = action.Id,
                                    FileUrl = _uploadService.UploadAsync(attachment)
                                });
                            }
                            await _repository.AddActionAttachmentAsync(attachments, action.Id);
                    }
                    break;
                case StepActions.SelectQutation:
                     //await _repository.SelectAttachmentAsync(request.refId.Value);
                    break;
                default:
                    break;
            }
            action.ActionNote = request.ActionNote;

            throw new NotImplementedException();
        }
    }
}
