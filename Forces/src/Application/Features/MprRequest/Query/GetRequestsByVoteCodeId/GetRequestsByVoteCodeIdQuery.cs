using Forces.Application.Features.MprRequest.Dto.Response;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services.Identity;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.MprRequest.Query.GetRequestsByVoteCodeId
{
    public class GetRequestsByVoteCodeIdQuery : IRequest<IResult<List<GetMprResponse>>>
    {
        [Required]
        public int Id { get; set; }
    }
    internal class GetRequestsByVoteCodeIdQueryHandler : IRequestHandler<GetRequestsByVoteCodeIdQuery, IResult<List<GetMprResponse>>>
    {
        private protected IMprRequestRepository _repository;
        private protected IUserService _userService;
        private protected IVoteCodeRepository _voteCodeRepository;

        public GetRequestsByVoteCodeIdQueryHandler(IMprRequestRepository repository, IUserService userService, IVoteCodeRepository voteCodeRepository)
        {
            _repository = repository;
            _userService = userService;
            _voteCodeRepository = voteCodeRepository;
        }

        public async Task<IResult<List<GetMprResponse>>> Handle(GetRequestsByVoteCodeIdQuery request, CancellationToken cancellationToken)
        {
            var Requests = await _repository.GetRequestsByVoteCodeAsync(request.Id);
            var users = (await _userService.GetAllAsync()).Data;
            var voteCodes = await _voteCodeRepository.GetAllVoteCodesAsync();
            try
            {
                var mapped = Requests.Select(x => new GetMprResponse()
                {
                    Attachments = x.Attachments.Select(x => x.FileUrl).ToList(),
                    LastActionDate = x.RequestActions.Max(z => z.LastModifiedOn ?? z.CreatedOn),
                    SelectedAttachment = x.Attachments.FirstOrDefault(z => z.Selected == true)?.FileUrl,
                    BaseId = x.BaseId,
                    BaseSectionId = x.SectionId,
                    CreationDate = x.CreatedOn,
                    CreatorId = x.CreatedBy,
                    CurrentStep = x.CurrentStep,
                    ForceId = x.ForceId,
                    PaiedOff = x.Paied,
                    ConfirmPaied = x.ConfirmPaied,
                    Id = x.Id,
                    isDone = x.isDone,
                    Name = $"{users.FirstOrDefault(z => z.Id == x.CreatedBy)?.FirstName} {users.FirstOrDefault(z => z.Id == x.CreatedBy)?.LastName}",
                    Note = x.RequestNote,
                    Priority = x.Priority,
                    RefrenceId = x.RequestRefranceCode,
                    RequestState = x.RequestState,
                    UserName = users.FirstOrDefault(z => z.Id == x.CreatedBy).UserName,
                    VoteCode = voteCodes[x.VoteCodeId] ?? "0",
                    SelectedAttachmentBy = x.Attachments.FirstOrDefault(z => z.Selected == true) != null ?
                $"{users.FirstOrDefault(z => z.Id == x.Attachments.FirstOrDefault(z => z.Selected == true)?.LastModifiedBy)?.FirstName} {users.FirstOrDefault(z => z.Id == x.Attachments.FirstOrDefault(z => z.Selected == true)?.LastModifiedBy)?.LastName} - {users.FirstOrDefault(z => z.Id == x.Attachments.FirstOrDefault(z => z.Selected == true)?.LastModifiedBy)?.UserName}"
                : string.Empty,
                    Actions = x.RequestActions.Select(a => new RequestActions()
                    {
                        ActionDate = a.LastModifiedOn ?? a.CreatedOn,
                        ActionNote = a.ActionNote,
                        ActionState = a.ActionState,
                        ActionStep = a.Step,
                        departId = a.TargetDepartId,
                        FullName = $"{users.FirstOrDefault(z => z.Id == a.LastModifiedBy)?.FirstName} {users.FirstOrDefault(z => z.Id == a.LastModifiedBy)?.LastName}",
                        Department = a.TargetDepartMentType,
                        Id = a.Id,
                        JobTitle = users.FirstOrDefault(z => z.Id == x.LastModifiedBy)?.JobTitle,
                        Rank = users.FirstOrDefault(z => z.Id == x.LastModifiedBy)?.Rank,
                        UserID = a.LastModifiedBy,
                        UserName = users.FirstOrDefault(z => z.Id == x.LastModifiedBy)?.UserName,
                        TakenAction = a.ActionState.ToString(),

                    }).ToList(),
                    Items = x.RequestItems.Select(i => new ItemDto()
                    {
                        ItemNameAR = i.ItemArName,
                        ItemCode = i.ItemCode,
                        ItemNSN = i.ItemNSN,
                        ItemName = i.ItemName,
                        ItemPrice = i.ItemPrice,
                        ItemQTY = i.ItemQty,
                        ItemUnit = i.Unit,

                    }).ToList()
                }).ToList();

                return await Result<List<GetMprResponse>>.SuccessAsync(mapped);
            }
            catch (Exception ex)
            {

                return await Result<List<GetMprResponse>>.FailAsync($"{ex.Message} \n inner: {ex.InnerException.Message}");
            }

        }
    }
}
