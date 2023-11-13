
using Forces.Domain.Contracts;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forces.Application.Models;
using Forces.Application.Enums;
using Forces.Domain.Dto;
using Forces.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Forces.Infrastructure.Models.Identity;
using System.Linq.Expressions;
using Forces.Application.Interfaces.Services;
using Forces.Application.Requests;
using System.IO;
using Forces.Application.Extensions;
using Forces.Application.Interfaces.Repositories;
using Forces.Infrastructure.Models;
using Forces.Infrastructure.Services.Requests;
using Forces.Application.Requests.Requests;

namespace Forces.Infrastructure.Concrete.Requests
{
    public abstract class BaseRequestConcrete<TRequest> : IRequestNotifications<TRequest>
        where TRequest : AuditableEntity<int>, IRequestModel<Appuser, Items, Priority, RequestAttachments<Models.VoteCodes, Appuser, Models.RequestActions>, RequestState, Models.RequestActions, Models.VoteCodes, RequestSteps>
    {
        public virtual TRequest RequestModel { get; set; }
        public virtual Models.RequestActions requestAction { get; set; }

        private protected ForcesDbContext _context;
        private protected ICurrentUserService _currentUserService;
        private protected IExcelService _excelService;
        private protected IUnitOfWork<int> _unitOfWork;
        private protected IUploadService _upLoadService;

        protected BaseRequestConcrete(ForcesDbContext context, ICurrentUserService currentUserService, IExcelService excelService, IUnitOfWork<int> unitOfWork, IUploadService upLoadService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _upLoadService = upLoadService;
        }
        public async Task<string> GetNextAvilableId<T>(int BaseId, int Digits = 6, int? year = null) where T : AuditableEntity<int>, TRequest
        {

            var Count = await _context.Set<T>().Where(x => x.CreatedOn.Year == DateTime.Now.Year && x.BaseId == BaseId).CountAsync();
            Count++;
            return Count.ToString($"d{Digits}"); // 000011
        }
        public async Task<List<TRequest>> GetAll()
        {
            return await _context.Set<TRequest>().ToListAsync();

        }
        public async Task<List<TRequest>> GetAllbyState(RequestState state)
        {
            return await _context.Set<TRequest>().Where(x => x.RequestState == state).ToListAsync();

        }
        public async Task<List<TRequest>> GetAllbySpicfications(Expression<Func<TRequest, bool>> condition)
        {
            return await _context.Set<TRequest>().Where(condition).ToListAsync();
        }

        public async Task<int> GetPendingCountByUser()
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId);
            var userStep = await GetUserStep(_currentUserService.UserId);
            return await _context.Set<TRequest>().Where(x => x.RequestState == RequestState.Pending && x.CurrentStep == userStep).CountAsync();
        }
        public async Task<int> GetPendingCountByUser(int? year = null)
        {
            year = year ?? DateTime.Now.Year;
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId);
            var userStep = await GetUserStep(_currentUserService.UserId);
            return await _context.Set<TRequest>().Where(x => x.RequestState == RequestState.Pending && x.CurrentStep == userStep && x.CreatedOn.Year == year).CountAsync();
        }
        public async Task<int> GetCompletedCountByUser(int? year = null)
        {
            year = year ?? DateTime.Now.Year;
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId);
            var userStep = await GetUserStep(_currentUserService.UserId);
            return await _context.Set<TRequest>().Where(x => x.RequestState == RequestState.Completed && x.CurrentStep == userStep && x.CreatedOn.Year == year).CountAsync();
        }
        public virtual List<int> AvilableSteps()
        {
            List<RequestSteps> Steps = new List<RequestSteps>()
            {
                RequestSteps.CreationStep,RequestSteps.OCLogStep,RequestSteps.DepartmentStep,
            };

            return new List<int>
            { 
                (int)RequestSteps.CreationStep,
                (int)RequestSteps.FASection,
                (int)RequestSteps.OCLogStep,
                (int)RequestSteps.OCPOdepo,
                (int)RequestSteps.OCLogdepo,
                (int)RequestSteps.ICPOSection ,
                (int)RequestSteps.DepartmentStep,
                (int)RequestSteps.OCDepartment,
                (int)RequestSteps.VoteCodeContreoller,
                (int)RequestSteps.OCFinance,
                (int)RequestSteps.WKLFinance,
                (int)RequestSteps.DFinanceStep,
                (int)RequestSteps.CommanderStep,
            };
        }
        public RequestSteps GetNextStep()
        {
            var steps = AvilableSteps();
            steps.Sort();
            var targetIndex = steps.IndexOf(steps.FirstOrDefault(x => x > (int)RequestModel.CurrentStep));
            return (RequestSteps)steps[targetIndex];
        }
        public async virtual Task<string> GenerateRefCodr(TRequest Model)
        {

            var CurrentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId);
            var Base = await _context.Bases.FirstOrDefaultAsync(x => x.Id == CurrentUser.BaseID);
            var SN = await GetNextAvilableId<TRequest>(Base.Id);
            var BaseCode = Base.BaseCode;
            var Year = DateTime.Now.ToString("yy");

            return $"{BaseCode}/{SN}/{Year}";
        }

        public async virtual Task<Result<string>> AddAsync(TRequest Model)
        {
            var CurrentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId);

            try
            {

                Model.RequestRefranceCode = await GenerateRefCodr(Model);
                Model.BaseId = CurrentUser.BaseID;
                Model.ForceId = CurrentUser.ForceID;
                Model.SectionId = CurrentUser.BaseSectionID;
                await _unitOfWork.Repository<TRequest>().AddAsync(Model);
                //  await _context.Set<TRequest>().AddAsync(Model);
                // await _context.SaveChangesAsync();
                await _unitOfWork.Commit(new System.Threading.CancellationToken());
                RequestModel = Model;//as Requests<Models.VoteCodes, Models.Identity.Appuser, Models.RequestActions>;
                await Execute(new ActionRequest() { ActionsType = ActionsType.Submit });
                // await _context.Database.CommitTransactionAsync();
                return await Result<string>.SuccessAsync(RequestModel.RequestRefranceCode, "Added Successfuly!");
            }
            catch (Exception ex)
            {
                // await _context.Database.RollbackTransactionAsync();
                await _unitOfWork.Rollback();
                return await Result<string>.FailAsync($"Error: {ex.Message}");
            }


        }
        public virtual RequestSteps GetStepByUserType(UserType userType)
        {
            switch (userType)
            {
                case UserType.Regular:
                    return RequestSteps.CreationStep;
                case UserType.RegularAdmin:
                    return RequestSteps.CreationStep;
                case UserType.OCLog:
                    return RequestSteps.OCLogStep;
                case UserType.OCLogAdmin:
                    return RequestSteps.OCLogStep;
                case UserType.Department:
                    return RequestSteps.DepartmentStep;
                case UserType.DepartmentAdmin:
                    return RequestSteps.OCDepartment;
                case UserType.HQ:
                    return RequestSteps.DepartmentStep;
                case UserType.Depot:
                    return RequestSteps.DepartmentStep;
                case UserType.VoteHolder:
                    return RequestSteps.VoteCodeContreoller;
                case UserType.DFINANCE:
                    return RequestSteps.DFinanceStep;
                case UserType.Commander:
                    return RequestSteps.CommanderStep;
                case UserType.BaseSectionAdmin:
                    return RequestSteps.OCLogStep;
                case UserType.BaseAdmin:
                    return RequestSteps.OCLogStep;
                case UserType.ForceAdmin:
                    return RequestSteps.OCLogStep;
                case UserType.SuperAdmin:
                    return RequestSteps.CreationStep;
                default:
                    return RequestSteps.CreationStep;
            }
        }
        public virtual List<ActionsType> GetActionsByUserType(UserType userType)
        {
            List<ActionsType> actions = new List<ActionsType>();
            switch (userType)
            {
                case UserType.Regular:
                    actions.Add(ActionsType.Canceld);
                    break;
                case UserType.RegularAdmin:
                    actions.Add(ActionsType.Escalate);
                    actions.Add(ActionsType.Reject);
                    break;
                case UserType.OCLog:
                    actions.Add(ActionsType.Escalate);
                    actions.Add(ActionsType.Reject);
                    actions.Add(ActionsType.Submit);
                    break;
                case UserType.OCLogAdmin:
                    actions.Add(ActionsType.Escalate);
                    actions.Add(ActionsType.Reject);
                    actions.Add(ActionsType.Submit);
                    break;
                case UserType.Department:
                    actions.Add(ActionsType.Escalate);
                    break;
                case UserType.DepartmentAdmin:
                    actions.Add(ActionsType.Escalate);
                    actions.Add(ActionsType.Reject);
                    actions.Add(ActionsType.Submit);
                    actions.Add(ActionsType.Redirect);
                    break;
                case UserType.HQ:
                    actions.Add(ActionsType.Escalate);
                    actions.Add(ActionsType.Reject);
                    actions.Add(ActionsType.Submit);
                    actions.Add(ActionsType.Redirect);
                    break;
                case UserType.Depot:
                    actions.Add(ActionsType.Escalate);
                    actions.Add(ActionsType.Reject);
                    actions.Add(ActionsType.Submit);
                    actions.Add(ActionsType.Redirect);
                    break;
                case UserType.VoteHolder:
                    actions.Add(ActionsType.Escalate);
                    actions.Add(ActionsType.Reject);
                    actions.Add(ActionsType.Submit);
                    actions.Add(ActionsType.Redirect);
                    break;
                case UserType.DFINANCE:
                    actions.Add(ActionsType.Escalate);
                    actions.Add(ActionsType.Reject);
                    actions.Add(ActionsType.Submit);
                    actions.Add(ActionsType.Redirect);
                    break;
                case UserType.Commander:
                    actions.Add(ActionsType.Reject);
                    actions.Add(ActionsType.Submit);
                    actions.Add(ActionsType.Redirect);
                    break;
                case UserType.BaseSectionAdmin:
                    break;
                case UserType.BaseAdmin:
                    break;
                case UserType.ForceAdmin:
                    break;
                case UserType.SuperAdmin:
                    break;
                default:
                    break;
            }
            return actions;
        }
        public virtual UserType GetUsersTypeByStep(RequestSteps Step)
        {
            switch (Step)
            {
                case RequestSteps.CreationStep:
                    return UserType.Regular;
                case RequestSteps.OCLogStep:
                    return UserType.OCLog;
                case RequestSteps.DepartmentStep:
                    return UserType.Department;
                case RequestSteps.OCDepartment:
                    return UserType.DepartmentAdmin;
                case RequestSteps.DFinanceStep:
                    return UserType.DFINANCE;
                case RequestSteps.VoteCodeContreoller:
                    return UserType.VoteHolder;
                case RequestSteps.CommanderStep:
                    return UserType.Commander;
                default:
                    return UserType.BaseAdmin;
            }
        }
        public async virtual Task<RequestSteps> GetUserStep(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user.UserType.HasValue)
            {
                switch (user.UserType)
                {
                    case UserType.Regular:
                        return RequestSteps.CreationStep;
                    case UserType.RegularAdmin:
                        return RequestSteps.CreationStep;
                    case UserType.OCLog:
                        return RequestSteps.OCLogStep;
                    case UserType.OCLogAdmin:
                        return RequestSteps.OCLogStep;
                    case UserType.Department:
                        return RequestSteps.DepartmentStep;
                    case UserType.DepartmentAdmin:
                        return RequestSteps.OCDepartment;
                    case UserType.HQ:
                        return RequestSteps.DepartmentStep;
                    case UserType.Depot:
                        return RequestSteps.DepartmentStep;
                    case UserType.VoteHolder:
                        return RequestSteps.VoteCodeContreoller;
                    case UserType.DFINANCE:
                        return RequestSteps.DFinanceStep;
                    case UserType.Commander:
                        return RequestSteps.CommanderStep;
                    case UserType.BaseSectionAdmin:
                        return RequestSteps.OCLogStep;
                    case UserType.BaseAdmin:
                        return RequestSteps.OCLogStep;
                    case UserType.ForceAdmin:
                        return RequestSteps.OCLogStep;
                    case UserType.SuperAdmin:
                        return RequestSteps.CreationStep;
                    default:
                        return RequestSteps.CreationStep;
                }
            }
            else
            {
                return RequestSteps.CreationStep;
            }

        }

        public async virtual Task<Result<bool>> Execute(ActionRequest Action, params RedirectModel[] redirectTo)
        {

            switch (Action.ActionsType)
            {
                case ActionsType.Submit:
                    return await Submit(Action);
                case ActionsType.Escalate:
                    return await Scale(Action);
                case ActionsType.Reject:
                    return await Reject(Action);
                case ActionsType.Redirect:
                    return await Redirect(redirectTo[0]);
                default:
                    return await Result<bool>.FailAsync("Something Went Wrong!");
            }

        }

        public virtual async Task<Result<bool>> Submit(ActionRequest Action)
        {
            var action = await _unitOfWork.Repository<RequestActions>().GetByIdAsync(Action.RequestActionId);
            var request = await _unitOfWork.Repository<TRequest>().GetByIdAsync(Action.RequestId);


            if (action != null)
            {
                requestAction = action;
                RequestModel = request;
                if (action != null)
                {

                    try
                    {
                        if (Action.VoteCodeId != null)
                        {
                            request.VoteCodeId = Action.VoteCodeId.Value;
                        }
                        action.ActionState = ActionState.Done;
                        action.ActionResult = ActionsType.Submit.ToString();
                        request.RequestState = RequestState.Completed;
                        request.CurrentStep = RequestSteps.VoteCodeContreoller;
                        await _unitOfWork.Repository<RequestActions>().UpdateAsync(action);
                        await _unitOfWork.Repository<TRequest>().UpdateAsync(request);
                        await _unitOfWork.Commit(new System.Threading.CancellationToken());
                        await NotifyOwner(request);
                        await NotifySteppers(request);
                        await _unitOfWork.Commit(new System.Threading.CancellationToken());
                        return await Result<bool>.SuccessAsync(true, "Request Submited Succesfuly!");
                    }
                    catch (Exception ex)
                    {
                        // await _context.Database.RollbackTransactionAsync();
                        await _unitOfWork.Rollback();
                        return await Result<bool>.FailAsync($"Error: {ex.Message}");
                    }

                }
                else
                {
                    return await Result<bool>.FailAsync("Error Not Found!");
                }
            }
            else
            {
                var NextAction = new Models.RequestActions()
                {
                    ActionState = ActionState.Pending,
                    BaseId = RequestModel.BaseId,
                    ForceId = RequestModel.ForceId,
                    Readed = false,
                    SectionId = RequestModel.SectionId,
                    RequestId = RequestModel.Id,
                    Step = GetNextStep(),
                    Seen = false,
                };
                RequestModel.CurrentStep = NextAction.Step;
                await _unitOfWork.Repository<RequestActions>().AddAsync(NextAction);
                await _unitOfWork.Repository<TRequest>().UpdateAsync(RequestModel);
                await _unitOfWork.Commit(new System.Threading.CancellationToken());
                await NotifyOwner(RequestModel);
                await NotifySteppers(RequestModel);
                await _unitOfWork.Commit(new System.Threading.CancellationToken());
                return await Result<bool>.SuccessAsync(true, "Request Submited Succesfuly!");
            }
        }

        public virtual async Task<Result<bool>> Scale(ActionRequest Action)
        {

            var action = await _unitOfWork.Repository<RequestActions>().GetByIdAsync(Action.RequestActionId);
            var request = await _unitOfWork.Repository<TRequest>().GetByIdAsync(Action.RequestId);
            requestAction = action;
            RequestModel = request;
            if (action != null)
            {

                try
                {
                    action.ActionNote = Action.ActionNote;
                    if (Action.Attachments.Count > 0)
                    {
                        action.Attachments = new List<RequestAttachments<Models.VoteCodes, Appuser, Models.RequestActions>>();


                        foreach (var attachment in Action.Attachments)
                        {
                            var att = new RequestAttachments<Models.VoteCodes, Appuser, Models.RequestActions>();
                            attachment.FileName = $"Q-{request.RequestRefranceCode.Replace("/", "-")}-{Guid.NewGuid()}{attachment.Extension}";
                            att.AttachmentType = "MPR-Qutation";
                            att.FileUrl = _upLoadService.UploadAsync(attachment);
                            att.RequestID = request.Id;
                            action.Attachments.Add(att);
                        }
                    }
                    if (Action.AdditionalPrice.HasValue)
                    {
                        if (Action.AdditionalPrice > 0)
                        {
                            action.ActionNote += $"|Price Update From {request.ItemPrice} To {Action.AdditionalPrice}";
                            request.ItemPrice = Action.AdditionalPrice.Value;
                        }
                    }
                    action.ActionState = ActionState.Done;
                    string ScaleLocation = "";
                    if (Action.DepartType != null)
                    {
                        ScaleLocation += $"Escalated To {Action.DepartType.ToDescriptionString()} ";
                        if (Action.DeparmentId != null)
                        {
                            if (Action.DepartType == DepartType.Depot)
                            {
                                var depo = _context.DepoDepartment.FirstOrDefault(x => x.Id == Action.DeparmentId.Value);
                                ScaleLocation += $"- {depo.Name}";
                            }
                            else
                            {
                                var HQ = _context.HQDepartment.FirstOrDefault(x => x.Id == Action.DeparmentId.Value);
                                ScaleLocation += $"- {HQ.Name}";
                            }
                        }

                    }
                    else
                    {
                        ScaleLocation = ActionsType.Escalate.ToString();
                    }
                    action.ActionResult = ScaleLocation;


                    var NextAction = new Models.RequestActions()
                    {
                        ActionState = ActionState.Pending,
                        BaseId = action.BaseId,
                        ForceId = action.ForceId,
                        Readed = false,
                        SectionId = action.SectionId,
                        RequestId = action.RequestId,
                        Step = GetNextStep(),
                        Seen = false,
                    };
                    if (Action.DepartType != null)
                    {
                        NextAction.TargetDepartMentType = Action.DepartType;
                    }
                    if (Action.DeparmentId != null)
                    {
                        NextAction.TargetDepartId = Action.DeparmentId;
                    }
                    if (!string.IsNullOrEmpty(Action.TargetUserId))
                    {
                        NextAction.TargetUserId = Action.TargetUserId;
                    }
                    request.CurrentStep = NextAction.Step;
                    await _unitOfWork.Repository<RequestActions>().AddAsync(NextAction);
                    await _unitOfWork.Repository<RequestActions>().UpdateAsync(action);
                    await _unitOfWork.Repository<TRequest>().UpdateAsync(request);
                    await _unitOfWork.Commit(new System.Threading.CancellationToken());
                    await NotifyOwner(request);
                    await NotifySteppers(request);
                    await _unitOfWork.Commit(new System.Threading.CancellationToken());

                    return await Result<bool>.SuccessAsync(true, "Request Escalated Succesfuly!");
                }
                catch (Exception ex)
                {

                    return await Result<bool>.FailAsync($"Error: {ex.Message}");
                }

            }
            else
            {
                return await Result<bool>.FailAsync("Error!");
            }
        }

        public virtual async Task<Result<bool>> Reject(ActionRequest Action)
        {
            var action = await _unitOfWork.Repository<RequestActions>().GetByIdAsync(Action.RequestActionId);
            var request = await _unitOfWork.Repository<TRequest>().GetByIdAsync(Action.RequestId);
            requestAction = action;
            RequestModel = request;
            if (action != null)
            {

                try
                {
                    action.ActionState = ActionState.Done;
                    action.ActionResult = ActionsType.Reject.ToString();
                    action.ActionNote = Action.ActionNote;
                    request.RequestState = RequestState.Rejected;
                    await _unitOfWork.Repository<RequestActions>().UpdateAsync(action);
                    await _unitOfWork.Repository<TRequest>().UpdateAsync(request);
                    await _unitOfWork.Commit(new System.Threading.CancellationToken());
                    await NotifyOwner(request);
                    await NotifySteppers(request);
                    await _unitOfWork.Commit(new System.Threading.CancellationToken());
                    return await Result<bool>.SuccessAsync(true, "Request Rejected Succesfuly!");
                }
                catch (Exception ex)
                {
                    await _context.Database.RollbackTransactionAsync();
                    return await Result<bool>.FailAsync($"Error: {ex.Message}");
                }

            }
            else
            {
                return await Result<bool>.FailAsync("ACTION Needed!");
            }
        }

        public virtual async Task<Result<bool>> Redirect(params RedirectModel[] redirectTo)
        {
            if (redirectTo.Length == 0)
            {
                return await Result<bool>.FailAsync("You Must Suply User To Redirect!");
            }
            else
            {

                var action = await _context.RequestActions.Include(x => x.Request).FirstOrDefaultAsync(x => x.Id == requestAction.Id);
                if (action != null)
                {
                    await _context.Database.BeginTransactionAsync();
                    try
                    {
                        foreach (var targetUser in redirectTo)
                        {
                            action.ActionState = requestAction.ActionState;
                            action.ActionResult = ActionsType.Redirect.ToString();
                            var NextAction = new Models.RequestActions()
                            {
                                ActionState = ActionState.Pending,
                                BaseId = action.BaseId,
                                ForceId = action.ForceId,
                                Readed = false,
                                SectionId = action.SectionId,
                                RequestId = action.RequestId,
                                Step = await GetUserStep(targetUser.TargetUserId),
                                Seen = false,
                                TargetUserId = targetUser.TargetUserId
                            };
                            action.Request.CurrentStep = NextAction.Step;
                            await _context.RequestActions.AddAsync(NextAction);
                        }

                        await _context.SaveChangesAsync();
                        await _context.Database.CommitTransactionAsync();
                    }
                    catch (Exception ex)
                    {
                        await _context.Database.RollbackTransactionAsync();
                        return await Result<bool>.FailAsync($"Error: {ex.Message}");
                    }

                }
                else
                {
                    return await Result<bool>.FailAsync("Error!");
                }


                return await Result<bool>.SuccessAsync(true, "Request Redirected Succesfuly!");
            }

        }

        public async Task NotifyOwner(TRequest request)
        {
            Type requestType = typeof(TRequest);
            if (request.CurrentStep > RequestSteps.OCLogStep)
            {
                var Existnotification = await _unitOfWork.Repository<NotificationsUsers>().Entities.FirstOrDefaultAsync(x => x.TargetUserId == request.CreatedBy && x.EntityPrimaryKey == request.Id.ToString() && x.NotificationType == typeof(TRequest).Name);
                if (Existnotification != null)
                {
                    Existnotification.Readed = false;
                    Existnotification.Seen = false;
                    Existnotification.Title = requestType == typeof(Models.Requests.NPRReguest) ? $"Your MPR State Updated" : $"Your {typeof(TRequest).Name} State Updated";
                    Existnotification.Description = request.RequestState == RequestState.Pending ? $"{request.RequestRefranceCode} Esclated To {request.CurrentStep.ToString()}" : request.RequestState == RequestState.Rejected ? $"{request.RequestRefranceCode} Rejected By {request.CurrentStep.ToString()}" : $"{request.RequestRefranceCode} Is Submitted And Completed";
                    Existnotification.CreatedOn = DateTime.Now;
                    await _unitOfWork.Repository<NotificationsUsers>().UpdateAsync(Existnotification);
                }
                else
                {

                    NotificationsUsers notification = new NotificationsUsers();
                    notification.EntityPrimaryKey = request.Id.ToString();
                    notification.NotificationType = typeof(TRequest).Name;
                    notification.TargetUserId = request.CreatedBy;
                    if (requestType == typeof(Models.Requests.NPRReguest))
                    {
                        notification.RefUrl = $"Requests/MPR/{notification.EntityPrimaryKey}";
                    }

                    notification.Readed = false;
                    notification.Seen = false;
                    notification.Title = requestType == typeof(Models.Requests.NPRReguest) ? $"Your MPR Request State Updated" : $"Your {typeof(TRequest).Name} Request State Updated";
                    notification.Description = request.RequestState == RequestState.Pending ? $"{request.RequestRefranceCode} Esclated To {request.CurrentStep.ToString()}" : request.RequestState == RequestState.Rejected ? $"{request.RequestRefranceCode} Rejected By {request.CurrentStep.ToString()}" : $"{request.RequestRefranceCode} Is Submitted And Colpeted";
                    await _unitOfWork.Repository<NotificationsUsers>().AddAsync(notification);
                }

            }

        }

        public async Task NotifySteppers(TRequest request)
        {

            var Existnotification = await _unitOfWork.Repository<NotificationsUsers>().Entities.Where(x => x.TargetUserId != request.CreatedBy && x.EntityPrimaryKey == request.Id.ToString() && x.NotificationType == typeof(TRequest).Name).ToListAsync();
            if (Existnotification.Count > 0)
            {
                Existnotification.ForEach(x =>
                {
                    x.Readed = true;
                    x.Seen = true;
                });
                foreach (var item in Existnotification)
                {
                    await _unitOfWork.Repository<NotificationsUsers>().UpdateAsync(item);
                }
            }
            var UsersStep = GetUsersTypeByStep(request.CurrentStep);
            List<Appuser> targetUsers = new List<Appuser>();
            if (request.CurrentStep == RequestSteps.DepartmentStep)
            {

                var Action = await _unitOfWork.Repository<Models.RequestActions>().Entities.FirstOrDefaultAsync(x => x.RequestId == request.Id && x.Step == RequestSteps.DepartmentStep);
                if (Action != null)
                {
                    if (Action.TargetDepartMentType == DepartType.Depot)
                    {
                        targetUsers = await _context.Users.Where(x => x.UserType == UsersStep && x.ForceID == request.ForceId && x.DepoDepartmentID == Action.TargetDepartId).ToListAsync();
                    }
                    else
                    {
                        targetUsers = await _context.Users.Where(x => x.UserType == UsersStep && x.ForceID == request.ForceId && x.HQDepartmentID == Action.TargetDepartId).ToListAsync();
                    }
                }

            }
            else if (request.CurrentStep == RequestSteps.VoteCodeContreoller)
            {
                //  targetUsers = await _context.Users.Include(x => x.HoldingVoteCodes).Where(x => x.DefaultVoteCodeID == request.VoteCodeId || x.HoldingVoteCodes.Any(y => y.Id == request.VoteCodeId)).ToListAsync();
                var VoteCodeHolders = await _context.VoteCodes.Include(x => x.VoteCodeHolders).Where(x => x.Id == request.VoteCodeId).Select(x => x.VoteCodeHolders.ToList()).FirstOrDefaultAsync();
                var DefaultHolder = await _context.VoteCodes.Include(x => x.DefaultHolder).Where(x => x.Id == request.VoteCodeId).Select(x => x.DefaultHolder).FirstOrDefaultAsync();
                targetUsers.AddRange(VoteCodeHolders);
                targetUsers.Add(DefaultHolder);
            }
            else
            {
                targetUsers = await _context.Users.Where(x => x.UserType == UsersStep && x.ForceID == request.ForceId && x.BaseID == request.BaseId).ToListAsync();

            }
            foreach (var user in targetUsers)
            {

                Type requestType = typeof(TRequest);
                NotificationsUsers notification = new NotificationsUsers();
                notification.EntityPrimaryKey = request.Id.ToString();
                notification.NotificationType = typeof(TRequest).Name;
                notification.TargetUserId = user.Id;
                if (requestType == typeof(Models.Requests.NPRReguest))
                {
                    notification.RefUrl = $"Requests/MPR/{notification.EntityPrimaryKey}";
                }

                notification.Readed = false;
                notification.Seen = false;
                notification.Title = requestType == typeof(Models.Requests.NPRReguest) ? $"New MPR Request" : $"New {typeof(TRequest).Name} Request";
                notification.Description = request.RequestState == RequestState.Pending ? $"{request.RequestRefranceCode} Esclated To {request.CurrentStep.ToString()}" : request.RequestState == RequestState.Rejected ? $"{request.RequestRefranceCode} Rejected By {request.CurrentStep.ToString()}" : $"{request.RequestRefranceCode} Is Submitted And Colpeted";
                await _unitOfWork.Repository<NotificationsUsers>().AddAsync(notification);
            }


        }

        public Task NotifyvCodeControllers(TRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
