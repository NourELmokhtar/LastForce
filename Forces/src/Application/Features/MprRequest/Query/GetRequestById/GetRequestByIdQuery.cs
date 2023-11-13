using Forces.Application.Enums;
using Forces.Application.Extensions;
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

namespace Forces.Application.Features.MprRequest.Query.GetRequestById
{
    public class GetRequestByIdQuery :IRequest<IResult<GetMprResponse>>
    {
        [Required]
        public int Id { get; set; }
    }
    internal class GetRequestByIdQueryHandler : IRequestHandler<GetRequestByIdQuery, IResult<GetMprResponse>>
    {
        private protected IMprRequestRepository _repository;
        private protected IUserService _userService;
        private protected IVoteCodeRepository _voteCodeRepository;

        public GetRequestByIdQueryHandler(IMprRequestRepository repository, IUserService userService, IVoteCodeRepository voteCodeRepository)
        {
            _repository = repository;
            _userService = userService;
            _voteCodeRepository = voteCodeRepository;
        }
       string GetRankName(int? rank)
        {
            if (rank.HasValue)
            {
            return ((RankType)rank).ToEnDescriptionString();

            }
            return string.Empty;
        }
        public async Task<IResult<GetMprResponse>> Handle(GetRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var Requests = await _repository.GetRequestByIdAsync(request.Id);
            var myList = new List<Models.MprRequest>();
            myList.Add(Requests);
            var users = (await _userService.GetAllAsync()).Data;
            var voteCodes = await _voteCodeRepository.GetAllVoteCodesAsync();
            var BasicInfo = await _repository.GerRequestBasecInfoAsync(Requests);
            try
            {
                var mapped = myList.Select(x => new GetMprResponse()
                {
                    Force = BasicInfo.Force,
                    Base = BasicInfo.Base,
                    Section = BasicInfo.Section,
                    Attachments = x.Attachments.Select(x => x.FileUrl).ToList(),
                    LastActionDate = x.RequestActions.Max(z => z.LastModifiedOn ?? z.CreatedOn),
                    SelectedAttachment = x.Attachments.FirstOrDefault(z => z.Selected == true)?.FileUrl,
                    BaseId = x.BaseId,
                    BaseSectionId = x.SectionId,
                    CreationDate = x.CreatedOn,
                    CreatorId = x.CreatedBy,
                    CurrentStep = x.CurrentStep,
                    ForceId = x.ForceId,
                    Id = x.Id,
                    isDone = x.isDone,
                    Name = $"{users.FirstOrDefault(z => z.Id == x.CreatedBy)?.FirstName} {users.FirstOrDefault(z => z.Id == x.CreatedBy)?.LastName}",
                    Note = x.RequestNote,
                    Priority = x.Priority,
                    RefrenceId = x.RequestRefranceCode,
                    PaiedOff = x.Paied,
                    ConfirmPaied = x.ConfirmPaied,
                    RequestState = x.RequestState,
                    UserName = users.FirstOrDefault(z => z.Id == x.CreatedBy).UserName,
                    Rank = users.FirstOrDefault(z => z.Id == x.CreatedBy).Rank,
                    strRank = ((RankType)users.FirstOrDefault(z => z.Id == x.CreatedBy)?.Rank).ToEnDescriptionString() ?? "",
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
                        JobTitle = users.FirstOrDefault(z => z.Id == a.LastModifiedBy)?.JobTitle,
                        Rank = users.FirstOrDefault(z => z.Id == a.LastModifiedBy)?.Rank,
                        strRank = GetRankName(users.FirstOrDefault(z => z.Id == a.LastModifiedBy)?.Rank),
                        UserID = a.LastModifiedBy,
                        UserName = users.FirstOrDefault(z => z.Id == a.LastModifiedBy)?.UserName,
                        TakenAction = a.ActionsType.ToString(),
                        ActionResult = a.ActionResult,
                        ActionAttachment = new List<string>(a.Attachments.Select(t=>t.FileUrl))

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
                        ItemId = i.ItemId,
                        VotecodeId = x.VoteCodeId

                    }).ToList()
                }).FirstOrDefault();

                return await Result<GetMprResponse>.SuccessAsync(mapped);
            }
            catch (Exception ex)
            {

                return await Result<GetMprResponse>.FailAsync($"{ex.Message} \n inner: {ex.InnerException.Message}");
            }

        }
    }
}
