using Forces.Application.Enums;
using Forces.Application.Extensions;
using Forces.Application.Features.MPRDashboard.Query.GetMPRData;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Application.Interfaces.Services.Identity;
using Forces.Application.Models;
using Forces.Application.Requests.Requests.NPRRequest;
using Forces.Application.Responses.Requets.MPR;
using Forces.Infrastructure.Concrete.Requests;
using Forces.Infrastructure.Contexts;
using Forces.Infrastructure.Models.Identity;
using Forces.Infrastructure.Models.Requests;
using Forces.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Services.Requests
{
    public class RequestService : BaseRequestConcrete<NPRReguest>, IRequestService<RequestService>
    {

        private protected IUploadService _upLoadService;
        private protected IUserService _userService;
        private protected ICurrentUserService _currentUserService;

        public RequestService(ForcesDbContext context,
            ICurrentUserService currentUserService,
            IExcelService excelService, IUploadService uploadService, IUnitOfWork<int> unitOfWork, IUserService userService) : base(context, currentUserService, excelService, unitOfWork, uploadService)
        {
            _upLoadService = uploadService;
            _userService = userService;
            _currentUserService = currentUserService;
        }

        public async Task<Result<string>> AddRequest(AddEditNPRRequest request)
        {
            if (request.ItemId == 0)
            {
                return await Result<string>.FailAsync("Please Supply An Item To Your Request");
            }
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == request.ItemId);

            NPRReguest req = new NPRReguest()
            {
                Priority = request.Priority,
                RequestState = Application.Enums.RequestState.Pending,
                VoteCodeId = request.VoteCodeId,
                CurrentStep = Application.Enums.RequestSteps.CreationStep,
                ItemId = request.ItemId,
                ItemArName = item.ItemArName,
                ItemName = item.ItemName,
                ItemCode = item.ItemCode,
                ItemNSN = item.ItemNsn,
                ItemPrice = request.Price,
                ItemQty = request.QTY,
                RequestNote = request.Note,
                Unit = request.Unit,
                ItemClass = item.ItemClass.ToDescriptionString(),

            };
            req.Attachments = new List<RequestAttachments<Models.VoteCodes, Appuser, Models.RequestActions>>();
            //if (request.Attachments.Count < 3)
            //{
            //    return await Result<string>.FailAsync("Attachment Must Be 3 At Leaset");
            //}
            var refranceID = await GenerateRefCodr(req);
            foreach (var attachment in request.Attachments)
            {

                var att = new RequestAttachments<Models.VoteCodes, Appuser, Models.RequestActions>();

                attachment.FileName = $"Q-{refranceID.Replace("/", "-")}-{Guid.NewGuid()}{attachment.Extension}";
                att.AttachmentType = "MPR";
                att.FileUrl = _upLoadService.UploadAsync(attachment);
                att.RequestID = req.Id;
                req.Attachments.Add(att);
            }
            return await AddAsync(req);
        }

        public async Task<Result<List<ActionsType>>> GetAvilableActions()
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId);
            if (user.UserType.HasValue)
            {
                return await Result<List<ActionsType>>.SuccessAsync(GetActionsByUserType(user.UserType.Value));
            }
            return await Result<List<ActionsType>>.SuccessAsync(new List<ActionsType>());
        }

        public async Task<Result<GetAllMPRResponse>> GetAllRequestsById(int requestId)
        {
            var MPR = await _unitOfWork.Repository<NPRReguest>().Entities.Include(x => x.VoteCode).Include(x => x.RequestActions).ThenInclude(x => x.Attachments).Include(x => x.Attachments).FirstOrDefaultAsync(x => x.Id == requestId);

            if (MPR != null)
            {
                var CreatorUer = await _context.Users.FirstOrDefaultAsync(x => x.Id == MPR.CreatedBy);

                GetAllMPRResponse response = new GetAllMPRResponse();
                response.BaseId = CreatorUer.BaseID;
                response.BaseSectionId = CreatorUer.BaseSectionID;
                response.CreationDate = MPR.CreatedOn;
                response.CurrentStep = MPR.CurrentStep;
                response.ForceId = CreatorUer.ForceID;
                response.Id = MPR.Id;
                response.ItemCode = MPR.ItemCode;
                response.ItemName = MPR.ItemName;
                response.ItemNameAR = MPR.ItemArName;
                response.ItemNSN = MPR.ItemNSN;
                response.ItemPrice = MPR.ItemPrice;
                response.ItemQTY = MPR.ItemQty;
                response.ItemUnit = MPR.Unit;
                response.Name = $"{CreatorUer.FirstName} {CreatorUer.LastName}";
                response.VoteCode = MPR.VoteCode.VoteCode;
                response.UserName = CreatorUer.UserName;
                response.RequestState = MPR.RequestState;
                response.Priority = MPR.Priority;
                response.Note = MPR.RequestNote;
                response.isDone = MPR.isDone;
                response.CreatorId = MPR.CreatedBy;
                response.LastActionDate = MPR.LastModifiedOn ?? MPR.CreatedOn;
                response.RefrenceId = MPR.RequestRefranceCode;
                response.Attachments = MPR.Attachments.Where(x => x.ActionId == null).Select(x => x.FileUrl).ToList();
                response.Actions = (from action in MPR.RequestActions.ToList()
                                    join user in _context.Users on action.LastModifiedBy equals user.Id
                                      into UsersTbl
                                    from user in UsersTbl.DefaultIfEmpty()
                                    select new RequestActions()
                                    {
                                        ActionDate = action.LastModifiedOn,
                                        ActionNote = action.ActionNote,
                                        ActionState = action.ActionState,
                                        TakenAction = action.ActionResult,
                                        FullName = $"{user?.FirstName} {user?.LastName}",
                                        Id = action.Id,
                                        ActionStep = action.Step,
                                        Department = action.TargetDepartMentType,
                                        departId = action.TargetDepartId,
                                        JobTitle = user?.JobTitle,
                                        Rank = user?.Rank,
                                        UserName = user?.UserName,
                                        UserID = action.LastModifiedBy == null ? string.Empty : action.LastModifiedBy,
                                        ActionAttachment = action.Attachments.Where(x => x.ActionId != null).Select(x => x.FileUrl).ToList()
                                    }).ToList();
                return await Result<GetAllMPRResponse>.SuccessAsync(response);
            }
            else
            {
                return await Result<GetAllMPRResponse>.FailAsync("Request Not Found");
            }

        }

        public async Task<Result<GetAllMPRResponse>> GetAllRequestsByRefrance(string Refrance)
        {
            var MPR = await _unitOfWork.Repository<NPRReguest>().Entities.Include(x => x.VoteCode).Include(x => x.RequestActions).Include(x => x.Attachments).FirstOrDefaultAsync(x => x.RequestRefranceCode == Refrance);

            if (MPR != null)
            {
                var CreatorUer = await _context.Users.FirstOrDefaultAsync(x => x.Id == MPR.CreatedBy);

                GetAllMPRResponse response = new GetAllMPRResponse();
                response.BaseId = CreatorUer.BaseID;
                response.BaseSectionId = CreatorUer.BaseSectionID;
                response.CreationDate = MPR.CreatedOn;
                response.CurrentStep = MPR.CurrentStep;
                response.ForceId = CreatorUer.ForceID;
                response.Id = MPR.Id;
                response.ItemCode = MPR.ItemCode;
                response.ItemName = MPR.ItemName;
                response.ItemNameAR = MPR.ItemArName;
                response.ItemNSN = MPR.ItemNSN;
                response.ItemPrice = MPR.ItemPrice;
                response.ItemQTY = MPR.ItemQty;
                response.ItemUnit = MPR.Unit;
                response.Name = $"{CreatorUer.FirstName} {CreatorUer.LastName}";
                response.VoteCode = MPR.VoteCode.VoteCode;
                response.UserName = CreatorUer.UserName;
                response.RequestState = MPR.RequestState;
                response.isDone = MPR.isDone;
                response.Priority = MPR.Priority;
                response.CreatorId = MPR.CreatedBy;
                response.RefrenceId = MPR.RequestRefranceCode;
                response.Note = MPR.RequestNote;
                response.Attachments = MPR.Attachments.Select(x => x.FileUrl).ToList();
                response.Actions = (from action in MPR.RequestActions.ToList()
                                    join user in _context.Users on action.LastModifiedBy equals user.Id
                                    into UsersTbl
                                    from user in UsersTbl.DefaultIfEmpty()
                                    select new RequestActions()
                                    {
                                        ActionDate = action.LastModifiedOn,
                                        ActionNote = action.ActionNote,
                                        ActionState = action.ActionState,
                                        TakenAction = action.ActionsType.ToString(),
                                        FullName = $"{user.FirstName} {user.LastName}",
                                        Id = action.Id,
                                        JobTitle = user.JobTitle,
                                        ActionStep = action.Step,
                                        Rank = user.Rank,
                                        UserName = user.UserName,
                                        UserID = action.LastModifiedBy
                                    }).OrderByDescending(x => x.ActionDate).ToList();
                return await Result<GetAllMPRResponse>.SuccessAsync(response);
            }
            else
            {
                return await Result<GetAllMPRResponse>.FailAsync("Request Not Found");
            }
        }
        public Expression<Func<NPRReguest, bool>> Getcondition(Application.Requests.Requests.GetRequestsBySpecificationsRequest Specifications)
        {
            Expression<Func<NPRReguest, bool>> condition = x => x.Id != 0;

            if (Specifications.BaseId != null)
            {
                condition = condition.And(x => x.BaseId == Specifications.BaseId);
            }
            if (!string.IsNullOrEmpty(Specifications.TargetUserId))
            {
                condition = condition.And(x => x.RequestActions.Any(y => y.TargetUserId == Specifications.TargetUserId));
            }
            if (Specifications.BaseSectionId != null)
            {
                condition = condition.And(x => x.SectionId == Specifications.BaseSectionId);
            }
            if (Specifications.userType != null)
            {
                var TargetStep = GetStepByUserType(Specifications.userType.Value);
                condition = condition.And(x => x.CurrentStep >= TargetStep);
            }
            if (Specifications.DateFrom != null)
            {
                condition = condition.And(x => x.CreatedOn >= Specifications.DateFrom);
            }
            if (Specifications.DateTo != null)
            {
                condition = condition.And(x => x.CreatedOn <= Specifications.DateTo);
            }
            if (Specifications.ForceId != null)
            {
                condition = condition.And(x => x.ForceId == Specifications.ForceId);
            }
            if (Specifications.RequestId != null)
            {
                condition = condition.And(x => x.Id == Specifications.RequestId);
            }
            if (Specifications.RequestStep != null)
            {
                condition = condition.And(x => x.CurrentStep == Specifications.RequestStep);
            }
            if (Specifications.Year != null)
            {
                condition = condition.And(x => x.CreatedOn.Year == Specifications.Year);
            }
            if (!string.IsNullOrEmpty(Specifications.UserId))
            {
                condition = condition.And(x => x.CreatedBy == Specifications.UserId);
            }
            if (!string.IsNullOrEmpty(Specifications.RequestRef))
            {
                condition = condition.And(x => x.RequestRefranceCode == Specifications.RequestRef);
            }
            if (Specifications.TargetDeparmentType != null)
            {
                condition = condition.And(x => x.RequestActions.Any(x => x.TargetDepartMentType == Specifications.TargetDeparmentType));

            }
            if (Specifications.TargetDepartmentId != null)
            {
                condition = condition.And(x => x.RequestActions.Any(x => x.TargetDepartId == Specifications.TargetDepartmentId));
            }
            return condition;
        }
        public async Task<Result<List<GetAllMPRResponse>>> GetAllRequestsBySpecifications(Application.Requests.Requests.GetRequestsBySpecificationsRequest Specifications)
        {
            Expression<Func<NPRReguest, bool>> condition = x => x.Id != 0;
            condition = Getcondition(Specifications);

            var MPR = await _unitOfWork.Repository<NPRReguest>().Entities.Include(x => x.VoteCode).Include(x => x.RequestActions).Include(x => x.Attachments).Where(condition).ToListAsync();
            var mappedRequests = (from request in MPR
                                  join CreatorUer in _context.Users on request.CreatedBy equals CreatorUer.Id
                                  select new GetAllMPRResponse()
                                  {

                                      BaseId = CreatorUer.BaseID,
                                      BaseSectionId = CreatorUer.BaseSectionID,
                                      CreationDate = request.CreatedOn,
                                      CurrentStep = request.CurrentStep,
                                      ForceId = CreatorUer.ForceID,
                                      Id = request.Id,
                                      RefrenceId = request.RequestRefranceCode,
                                      ItemCode = request.ItemCode,
                                      ItemName = request.ItemName,
                                      ItemNameAR = request.ItemArName,
                                      ItemNSN = request.ItemNSN,
                                      ItemPrice = request.ItemPrice,
                                      ItemQTY = request.ItemQty,
                                      isDone = request.isDone,
                                      ItemUnit = request.Unit,
                                      Name = $"{CreatorUer.FirstName} {CreatorUer.LastName}",
                                      VoteCode = request.VoteCode.VoteCode,
                                      UserName = CreatorUer.UserName,
                                      RequestState = request.RequestState,
                                      Priority = request.Priority,
                                      CreatorId = request.CreatedBy,
                                      Note = request.RequestNote,
                                      LastActionDate = request.LastModifiedOn ?? request.CreatedOn,
                                      Attachments = request.Attachments.Where(x => x.ActionId == null).Select(x => x.FileUrl).ToList(),
                                  }).OrderByDescending(x => x.LastActionDate).ToList();

            return await Result<List<GetAllMPRResponse>>.SuccessAsync(mappedRequests);
        }

        public async Task<Result<List<GetAllMPRResponse>>> GetAllRequestsBySteps(RequestSteps Step)
        {
            var MPR = await _unitOfWork.Repository<NPRReguest>().Entities.Include(x => x.VoteCode).Include(x => x.RequestActions).Include(x => x.Attachments).Where(x => x.CurrentStep == Step).ToListAsync();
            var mappedRequests = (from request in MPR
                                  join CreatorUer in _context.Users on request.CreatedBy equals CreatorUer.Id
                                  select new GetAllMPRResponse()
                                  {

                                      BaseId = CreatorUer.BaseID,
                                      BaseSectionId = CreatorUer.BaseSectionID,
                                      CreationDate = request.CreatedOn,
                                      CurrentStep = request.CurrentStep,
                                      ForceId = CreatorUer.ForceID,
                                      Id = request.Id,
                                      ItemCode = request.ItemCode,
                                      ItemName = request.ItemName,
                                      ItemNameAR = request.ItemArName,
                                      ItemNSN = request.ItemNSN,
                                      ItemPrice = request.ItemPrice,
                                      ItemQTY = request.ItemQty,
                                      ItemUnit = request.Unit,
                                      isDone = request.isDone,
                                      RefrenceId = request.RequestRefranceCode,
                                      Name = $"{CreatorUer.FirstName} {CreatorUer.LastName}",
                                      VoteCode = request.VoteCode.VoteCode,
                                      UserName = CreatorUer.UserName,
                                      RequestState = request.RequestState,
                                      LastActionDate = request.LastModifiedOn ?? request.CreatedOn,
                                      Priority = request.Priority,
                                      CreatorId = request.CreatedBy,
                                      Note = request.RequestNote,
                                      Attachments = request.Attachments.Select(x => x.FileUrl).ToList(),
                                  }).OrderByDescending(x => x.LastActionDate).ToList();
            return await Result<List<GetAllMPRResponse>>.SuccessAsync(mappedRequests);
        }

        public async Task<Result<List<GetAllMPRResponse>>> GetAllRequestsByUser(string UserId)
        {
            var MPR = await _unitOfWork.Repository<NPRReguest>().Entities.Include(x => x.VoteCode).Include(x => x.RequestActions).Include(x => x.Attachments).Where(x => x.CreatedBy == UserId).ToListAsync();
            var mappedRequests = (from request in MPR
                                  join CreatorUer in _context.Users on request.CreatedBy equals CreatorUer.Id
                                  select new GetAllMPRResponse()
                                  {

                                      BaseId = CreatorUer.BaseID,
                                      BaseSectionId = CreatorUer.BaseSectionID,
                                      CreationDate = request.CreatedOn,
                                      CurrentStep = request.CurrentStep,
                                      ForceId = CreatorUer.ForceID,
                                      Id = request.Id,
                                      ItemCode = request.ItemCode,
                                      ItemName = request.ItemName,
                                      ItemNameAR = request.ItemArName,
                                      ItemNSN = request.ItemNSN,
                                      ItemPrice = request.ItemPrice,
                                      ItemQTY = request.ItemQty,
                                      isDone = request.isDone,
                                      ItemUnit = request.Unit,
                                      RefrenceId = request.RequestRefranceCode,
                                      Name = $"{CreatorUer.FirstName} {CreatorUer.LastName}",
                                      VoteCode = request.VoteCode.VoteCode,
                                      UserName = CreatorUer.UserName,
                                      RequestState = request.RequestState,
                                      Priority = request.Priority,
                                      CreatorId = request.CreatedBy,
                                      LastActionDate = request.LastModifiedOn ?? request.CreatedOn,
                                      Note = request.RequestNote,
                                      Attachments = request.Attachments.Select(x => x.FileUrl).ToList(),
                                  }).OrderByDescending(x => x.LastActionDate).ToList();
            return await Result<List<GetAllMPRResponse>>.SuccessAsync(mappedRequests);
        }

        public async Task<Result<int>> GetAllRequestsCount(Application.Requests.Requests.GetRequestsBySpecificationsRequest Specifications)
        {
            Expression<Func<NPRReguest, bool>> condition = x => x.Id != 0;
            condition = Getcondition(Specifications);

            var Count = await _unitOfWork.Repository<NPRReguest>().Entities.Include(x => x.VoteCode).Include(x => x.RequestActions).Include(x => x.Attachments).Where(condition).CountAsync();
            return await Result<int>.SuccessAsync(Count);
        }

        public async Task<Result<List<GetAllMPRResponse>>> GetAllRequestsForTargetUser(string UserId)
        {
            var TargetAction = await _context.RequestActions.Include(x => x.Request).Where(x => x.TargetUserId == UserId).ToListAsync();
            var MPR = TargetAction.Select(x => x.Request).OfType<NPRReguest>().ToList();
            var mappedRequests = (from request in MPR
                                  join CreatorUer in _context.Users on request.CreatedBy equals CreatorUer.Id
                                  select new GetAllMPRResponse()
                                  {

                                      BaseId = CreatorUer.BaseID,
                                      BaseSectionId = CreatorUer.BaseSectionID,
                                      CreationDate = request.CreatedOn,
                                      CurrentStep = request.CurrentStep,
                                      ForceId = CreatorUer.ForceID,
                                      Id = request.Id,
                                      ItemCode = request.ItemCode,
                                      ItemName = request.ItemName,
                                      ItemNameAR = request.ItemArName,
                                      ItemNSN = request.ItemNSN,
                                      ItemPrice = request.ItemPrice,
                                      isDone = request.isDone,
                                      ItemQTY = request.ItemQty,
                                      ItemUnit = request.Unit,
                                      Name = $"{CreatorUer.FirstName} {CreatorUer.LastName}",
                                      VoteCode = request.VoteCode.VoteCode,
                                      UserName = CreatorUer.UserName,
                                      RequestState = request.RequestState,
                                      Priority = request.Priority,
                                      LastActionDate = request.LastModifiedOn ?? request.CreatedOn,
                                      CreatorId = request.CreatedBy,
                                      RefrenceId = request.RequestRefranceCode,
                                      Note = request.RequestNote,
                                      Attachments = request.Attachments.Select(x => x.FileUrl).ToList(),
                                  }).OrderByDescending(x => x.LastActionDate).ToList();
            return await Result<List<GetAllMPRResponse>>.SuccessAsync(mappedRequests);

        }

        public async Task<Result<List<GetAllMPRResponse>>> GetAllRequestsToLog()
        {
            var CurrentUserStep = await _userService.GetStepByUserId(_currentUserService.UserId);
            var CurrentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId);
            var MPR = await _unitOfWork.Repository<NPRReguest>().Entities.Include(x => x.VoteCode).Include(x => x.RequestActions).Include(x => x.Attachments).Where(x => x.CurrentStep >= CurrentUserStep && x.ForceId == CurrentUser.ForceID && x.BaseId == CurrentUser.BaseID).ToListAsync();
            var mappedRequests = (from request in MPR
                                  join CreatorUer in _context.Users on request.CreatedBy equals CreatorUer.Id
                                  select new GetAllMPRResponse()
                                  {

                                      BaseId = CreatorUer.BaseID,
                                      BaseSectionId = CreatorUer.BaseSectionID,
                                      CreationDate = request.CreatedOn,
                                      CurrentStep = request.CurrentStep,
                                      ForceId = CreatorUer.ForceID,
                                      Id = request.Id,
                                      ItemCode = request.ItemCode,
                                      ItemName = request.ItemName,
                                      ItemNameAR = request.ItemArName,
                                      ItemNSN = request.ItemNSN,
                                      ItemPrice = request.ItemPrice,
                                      isDone = request.isDone,
                                      ItemQTY = request.ItemQty,
                                      ItemUnit = request.Unit,
                                      RefrenceId = request.RequestRefranceCode,
                                      Name = $"{CreatorUer.FirstName} {CreatorUer.LastName}",
                                      VoteCode = request.VoteCode.VoteCode,
                                      UserName = CreatorUer.UserName,
                                      RequestState = request.RequestState,
                                      Priority = request.Priority,
                                      Note = request.RequestNote,
                                      LastActionDate = request.LastModifiedOn ?? request.CreatedOn,
                                      CreatorId = request.CreatedBy,
                                      Attachments = request.Attachments.Select(x => x.FileUrl).ToList(),
                                      //Actions = (from action in request.RequestActions.Where(x => x.Step == CurrentUserStep)
                                      //           join user in _context.Users on action.CreatedBy equals user.Id
                                      //           select new RequestActions()
                                      //           {
                                      //               ActionDate = action.LastModifiedOn,
                                      //               ActionNote = action.ActionNote,
                                      //               ActionState = action.ActionState,
                                      //               TakenAction = action.ActionsType.ToString(),
                                      //               FullName = $"{user.FirstName} {user.LastName}",
                                      //               Id = action.Id,
                                      //               JobTitle = user.JobTitle,
                                      //               Rank = user.Rank,
                                      //               UserName = user.UserName
                                      //           }).ToList()
                                  }).OrderByDescending(x => x.LastActionDate).ToList();
            return await Result<List<GetAllMPRResponse>>.SuccessAsync(mappedRequests);

        }

        public async Task<Result<bool>> SubmitAction(Application.Requests.Requests.ActionRequest action)
        {
            return await Execute(action);
        }

        public async Task<Result<GetMPRDashboardDataResponse>> GetMPRDashboardData()
        {
            GetMPRDashboardDataResponse Model = new GetMPRDashboardDataResponse();
            var BaseId = await _userService.GetBaseID(_currentUserService.UserId);
            if (BaseId.HasValue)
            {
                var Request = await _context.MprRequests.Where(x => x.BaseId == BaseId).Select(x => new { x.RequestState,x.isDone,x.Paied,x.ConfirmPaied }).ToListAsync();
          
                var State = Request.Select(x => x.RequestState);
             var PayedOff = Request.Select(x => x.Paied);
                Model.Pending = State.Where(x => x == RequestState.Pending).Count();
                Model.Completed = State.Where(x => x == RequestState.Completed).Count();
                Model.Rejected = State.Where(x => x == RequestState.Rejected).Count();
                Model.Canceld = State.Where(x => x == RequestState.Canceldeld).Count();
                Model.PaiedOff = PayedOff.Where(x => x == true).Count();
                Model.TotalRequests = Request.Count();
            }
            else
            {
                var ForceId = await _userService.GetForceID(_currentUserService.UserId);
                var Request = await _context.MprRequests.Where(x => x.ForceId == ForceId).Select(x => new { x.RequestState, x.isDone, x.Paied, x.ConfirmPaied }).ToListAsync();

                var State = Request.Select(x => x.RequestState);
                var PayedOff = Request.Select(x => x.Paied);
                Model.Pending = State.Where(x => x == RequestState.Pending).Count();
                Model.Completed = State.Where(x => x == RequestState.Completed).Count();
                Model.Rejected = State.Where(x => x == RequestState.Rejected).Count();
                Model.Canceld = State.Where(x => x == RequestState.Canceldeld).Count();
                Model.PaiedOff = PayedOff.Where(x => x == true).Count();
                Model.TotalRequests = Request.Count();
            }

            return await Result<GetMPRDashboardDataResponse>.SuccessAsync(Model);

        }
    }

}
