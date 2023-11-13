using Forces.Application.Enums;
using Forces.Application.Extensions;
using Forces.Application.Features.MprRequest.Dto.Request;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Application.Models;
using Forces.Application.Requests;
using Forces.Application.Requests.VoteCodes;
using Forces.Domain.Contracts;
using Forces.Infrastructure.Contexts;
using Forces.Infrastructure.Models;
using Forces.Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Repositories
{
    public class MprRequestRepository : IMprRequestRepository
    {
        private protected readonly ICurrentUserService _currentUserService;
        private Appuser currentUser;
        private protected ForcesDbContext _context;
        private protected readonly MprStepTypes mprStepTypes;
        private readonly IUploadService _uploadService;
        private readonly IVoteCodeService _voteCodeService;
        private readonly UserManager<Appuser> _userManager;

        public MprRequestRepository(ICurrentUserService currentUserService, ForcesDbContext context, MprStepTypes mprStepTypes, IUploadService uploadService, IVoteCodeService voteCodeService, UserManager<Appuser> userManager)
        {
            _currentUserService = currentUserService;
            _context = context;
            this.currentUser = _context.Users.AsNoTracking().FirstOrDefault(x => x.Id == _currentUserService.UserId);
            this.mprStepTypes = mprStepTypes;
            _uploadService = uploadService;
            _voteCodeService = voteCodeService;
            _userManager = userManager;
        }

        public async Task<int> AddActionAsync(MprRequestAction action, int RequestId , RequestState requestState)
        {
            var request = await _context.MprRequests.FirstOrDefaultAsync(x => x.Id == RequestId);
             
            request.RequestState = requestState;
            request.CurrentStep = action.Step;
            action.ForceId = currentUser.ForceID; 
            action.SectionId = request.SectionId;
            action.BaseId = request.BaseId;
            action.Readed = false;
            action.Seen = false;
            action.RequestId = RequestId;
            action.CreatedBy = currentUser.Id;
            action.CreatedOn = DateTime.Now;

            _context.MprRequestAction.Add(action);
            //request.RequestActions.Add(action);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddActionAttachmentAsync(MprRequestAttachments attachment, int ActionId)
        {
            var action = await _context.MprRequestAction.FirstOrDefaultAsync(x => x.Id == ActionId);
            action.Attachments.Add(attachment);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddActionAttachmentAsync(List<MprRequestAttachments> attachments, int ActionId)
        {
            var action = await _context.MprRequestAction.FirstOrDefaultAsync(x => x.Id == ActionId);
            foreach (var attachment in attachments)
            {
                attachment.CreatedBy = currentUser.Id;
                attachment.CreatedOn = DateTime.Now;
                action.Attachments.Add(attachment);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddAttachmentAsync(MprRequestAttachments attachment, int RequestId)
        {
            var request = await _context.MprRequests.FirstOrDefaultAsync(x => x.Id == RequestId);

            request.Attachments.Add(attachment);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddAttachmentsAsync(List<MprRequestAttachments> attachments, int RequestId)
        {
            var request = await _context.MprRequests.FirstOrDefaultAsync(x => x.Id == RequestId);
            foreach (var attachment in attachments)
            {
                attachment.CreatedBy = currentUser.Id;
                attachment.CreatedOn = DateTime.Now;
                request.Attachments.Add(attachment);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddRequestAsync(MprRequest mpr)
        {
            mpr.RequestRefranceCode = await GenerateRefCodr();
            mpr.ForceId = currentUser.ForceID;
            mpr.BaseId = currentUser.BaseID;
            mpr.SectionId = currentUser.BaseSectionID;
            mpr.CreatedOn = DateTime.Now;
            mpr.CreatedBy = currentUser.Id;
            _context.MprRequests.Add(mpr);
            return await _context.SaveChangesAsync();
        }

     

        public async Task<MprSteps> GerRequestStepAsync(int RequestId)
        {
            var request = await _context.MprRequests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == RequestId);
            return request.CurrentStep;
        }

        public async Task<List<MprRequest>> GetAllRequestsAsync()
        {
            return await _context.MprRequests.AsNoTracking().ToListAsync();
        }

        public async Task<List<MprRequest>> GetAllRequestsByBaseIdAsync()
        {
            if (currentUser.BaseID.HasValue)
            {
                return await _context.MprRequests.AsNoTracking().Where(x => x.BaseId == currentUser.BaseID).ToListAsync();
            }
            return new List<MprRequest>();
        }

        public async Task<List<MprRequest>> GetAllRequestsByForceIdAsync()
        {
            if (currentUser.ForceID.HasValue)
            {
                return await _context.MprRequests.AsNoTracking().Where(x => x.ForceId == currentUser.ForceID).ToListAsync();
            }
            return new List<MprRequest>();
        }

        public async Task<List<MprRequest>> GetAllRequestsBySectionIdAsync()
        {
            if (currentUser.BaseSectionID.HasValue)
            {
                return await _context.MprRequests.AsNoTracking().Where(x => x.SectionId == currentUser.BaseSectionID).ToListAsync();
            }
            return new List<MprRequest>();
        }

        public async Task<List<MprRequest>> GetAllRequestsByUserIdAsync(string UserID)
        {
            return await _context.MprRequests.AsNoTracking().Where(x => x.CreatedBy == UserID).ToListAsync();
        }

        public async Task<MprRequest> GetRequestByIdAsync(int id)
        {
            return await _context.MprRequests.Include(x=>x.Attachments).Include(x=>x.RequestItems).Include(x=>x.RequestActions).ThenInclude(x=>x.Attachments).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<MprRequest> GetRequestByRefrenceIdAsync(string RefId)
        {
            return await _context.MprRequests.AsNoTracking().FirstOrDefaultAsync(x => x.RequestRefranceCode == RefId);
        }
        public async Task<(string Force,string Base, string Section)> GerRequestBasecInfoAsync(MprRequest request)
        {
            string Force = "";
            string Base = "";
            string Section = "";
            if (request.ForceId.HasValue)
            {
                var ForceEntity = await _context.Forces.Include(x => x.Bases).ThenInclude(x => x.Sections).FirstOrDefaultAsync(x => x.Id == request.ForceId.Value);
                Force = ForceEntity.ForceName;
                if (request.BaseId.HasValue)
                {
                    var BaseEntity = ForceEntity.Bases.FirstOrDefault(x => x.Id == request.BaseId.Value);
                    Base = BaseEntity.BaseName;
                    if (request.SectionId.HasValue)
                    {
                        var SectionEntity = BaseEntity.Sections.FirstOrDefault(x => x.Id == request.SectionId.Value);
                        Section = SectionEntity.SectionName;
                    }
                }

            }
            return (Force, Base, Section);
        }
        public async Task<List<MprRequest>> GetRequestsAsync()
        {
            var UserStep = mprStepTypes.GetUserStep(currentUser.UserType.Value);
            var userScope = mprStepTypes.userTypeScope[currentUser.UserType.Value];
            Expression<Func<MprRequest, bool>> cond = x => x.Id > 0;
            switch (userScope)
            {
                case Scope.Force:
                    if (currentUser.ForceID.HasValue)
                    {
                        cond = cond.And(x => x.ForceId == currentUser.ForceID.Value);
                    }
                    break;
                case Scope.Base:
                    if (currentUser.BaseID.HasValue)
                    {
                        cond = cond.And(x => x.BaseId == currentUser.BaseID.Value);
                    }
                    break;
                case Scope.Section:
                    if (currentUser.BaseSectionID.HasValue)
                    {
                        cond = cond.And(x => x.SectionId == currentUser.BaseSectionID.Value);
                    }
                    break;
                default:
                    break;
            }
            cond = cond.And(x => x.CurrentStep == UserStep);
            if (currentUser.UserType.Value == UserType.SuperAdmin)
            {
                return await _context.MprRequests.Include(x => x.Attachments).Include(x => x.RequestActions)
              .Include(x => x.RequestItems).AsNoTracking().ToListAsync();
            }
            cond = cond.Or(x => x.RequestActions.Any(z => z.LastModifiedBy == currentUser.Id || z.CreatedBy == currentUser.Id));
            return await _context.MprRequests.Include(x=>x.Attachments).Include(x=>x.RequestActions)
                .Include(x=>x.RequestItems).AsNoTracking()
                .Where(cond).ToListAsync();
        }

 

        public async Task<string> GetNextAvilableId(int BaseId, int Digits = 6, int? year = null)
        {

            var Count = await _context.MprRequests.Where(x => x.CreatedOn.Year == DateTime.Now.Year && x.BaseId == BaseId).CountAsync();
            Count++;
            return Count.ToString($"d{Digits}"); // 000011
        }

        public async virtual Task<string> GenerateRefCodr()
        {

            var Base = await _context.Bases.FirstOrDefaultAsync(x => x.Id == currentUser.BaseID);
            var SN = await GetNextAvilableId(Base.Id);
            var BaseCode = Base.BaseCode;
            var Year = DateTime.Now.ToString("yy");

            return $"MPR/{BaseCode}/{SN}/{Year}";
        }

        public async Task<List<StepActions>> GetAvilableActionsAsync(int RequestId)
        {
            var CurrentStep = await _context.MprRequests.AsNoTracking().Select(x => new { x.Id, x.CurrentStep }).FirstOrDefaultAsync(x => x.Id == RequestId);
            return mprStepTypes.StepsActions[CurrentStep.CurrentStep].ToList();
        }

        public MprSteps GetNextStep(MprRequest request)
        {
            var steps = Enum.GetValues(typeof(MprSteps)).Cast<MprSteps>().Cast<int>().ToList();
            steps.Sort();
            var targetIndex = steps.IndexOf(steps.FirstOrDefault(x => x > (int)request.CurrentStep));
            return (MprSteps)steps[targetIndex];
        }

        public async Task<int> NotifyOwner(MprRequest request)
        {
            Type requestType = typeof(MprRequest);
            if (request.CurrentStep > 0)
            {
                var Existnotification = await _context.NotificationsUsers.FirstOrDefaultAsync(x => x.TargetUserId == request.CreatedBy && x.EntityPrimaryKey == request.Id.ToString() && x.NotificationType == typeof(MprRequest).Name);
                if (Existnotification != null)
                {
                    Existnotification.Readed = false;
                    Existnotification.Seen = false;
                    Existnotification.Title = $"Your MPR State Updated"  ;
                    Existnotification.Description = request.RequestState == RequestState.Pending ? $"{request.RequestRefranceCode} Esclated To {request.CurrentStep.ToString()}" : request.RequestState == RequestState.Rejected ? $"{request.RequestRefranceCode} Rejected By {request.CurrentStep.ToString()}" : $"{request.RequestRefranceCode} Is Submitted And Completed";
                    Existnotification.CreatedOn = DateTime.Now;
                    return await _context.SaveChangesAsync();
                }
                else
                {

                    NotificationsUsers notification = new NotificationsUsers();
                    notification.EntityPrimaryKey = request.Id.ToString();
                    notification.NotificationType = typeof(MprRequest).Name;
                    notification.TargetUserId = request.CreatedBy;
                    
                        notification.RefUrl = $"Requests/MPR/{notification.EntityPrimaryKey}";
                    

                    notification.Readed = false;
                    notification.Seen = false;
                    notification.Title = $"Your MPR Request State Updated";
                    notification.Description = request.RequestState == RequestState.Pending ? $"{request.RequestRefranceCode} Esclated To {request.CurrentStep.ToString()}" : request.RequestState == RequestState.Rejected ? $"{request.RequestRefranceCode} Rejected By {request.CurrentStep.ToString()}" : $"{request.RequestRefranceCode} Is Submitted And Colpeted";
                    notification.CreatedBy = currentUser.Id;
                    notification.CreatedOn = DateTime.Now;
                    _context.NotificationsUsers.Add(notification);
                    return await _context.SaveChangesAsync();
                }

            }
            return 0;
        }

        public async Task<int> NotifySteppers(MprRequest request)
        {
            var Existnotification = await _context.NotificationsUsers.Where(x => x.TargetUserId != request.CreatedBy && x.EntityPrimaryKey == request.Id.ToString() && x.NotificationType == typeof(MprRequest).Name).ToListAsync();
            if (Existnotification.Count > 0)
            {
                Existnotification.ForEach(x =>
                {
                    x.Readed = true;
                    x.Seen = true;
                });
                return await _context.SaveChangesAsync();
            }
         
            var UsersStep = mprStepTypes.UsersSteps[request.CurrentStep];
            List<Appuser> targetUsers = new List<Appuser>();
            if (request.CurrentStep == MprSteps.DepartmentStep)
            {

                var Action = await _context.MprRequestAction.FirstOrDefaultAsync(x => x.RequestId == request.Id && x.Step == MprSteps.DepartmentStep);
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
            else if (request.CurrentStep == MprSteps.VoteCodeContreoller)
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

               
                NotificationsUsers notification = new NotificationsUsers();
                notification.EntityPrimaryKey = request.Id.ToString();
                notification.NotificationType = typeof(MprRequest).Name;
                notification.TargetUserId = user.Id; 
                notification.RefUrl = $"Requests/MPR/{notification.EntityPrimaryKey}";
               

                notification.Readed = false;
                notification.Seen = false;
                notification.Title = $"New MPR Request"  ;
                notification.Description = request.RequestState == RequestState.Pending ? $"{request.RequestRefranceCode} Esclated To {request.CurrentStep.ToString()}" : request.RequestState == RequestState.Rejected ? $"{request.RequestRefranceCode} Rejected By {request.CurrentStep.ToString()}" : $"{request.RequestRefranceCode} Is Submitted And Colpeted";
               notification.CreatedBy = currentUser.Id;
               notification.CreatedOn = DateTime.Now;
                await _context.NotificationsUsers.AddAsync(notification);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> NotifyStepperUser(MprRequest request, string TargetUser)
        {
            var Existnotification = await _context.NotificationsUsers.Where(x => x.TargetUserId != request.CreatedBy && x.EntityPrimaryKey == request.Id.ToString() && x.NotificationType == typeof(MprRequest).Name).ToListAsync();
            if (Existnotification.Count > 0)
            {
                Existnotification.ForEach(x =>
                {
                    x.Readed = true;
                    x.Seen = true;
                });
                return await _context.SaveChangesAsync();
            }

            
            NotificationsUsers notification = new NotificationsUsers();
            notification.EntityPrimaryKey = request.Id.ToString();
            notification.NotificationType = typeof(MprRequest).Name;
            notification.TargetUserId = TargetUser;
            notification.RefUrl = $"Requests/MPR/{notification.EntityPrimaryKey}";
            notification.Readed = false;
            notification.Seen = false;
            notification.Title =  $"New MPR Request";
            notification.Description = request.RequestState == RequestState.Pending ? $"{request.RequestRefranceCode} Assined To you!" : request.RequestState == RequestState.Rejected ? $"{request.RequestRefranceCode} Rejected By {request.CurrentStep.ToString()}" : $"{request.RequestRefranceCode} Is Submitted And Colpeted";
            notification.CreatedBy = currentUser.Id;
            notification.CreatedOn = DateTime.Now;
            await _context.NotificationsUsers.AddAsync(notification);
            return await _context.SaveChangesAsync();


        }

        public async Task<int> CancelRequestAsync(int RequestId, string Note)
        {
            var request = await _context.MprRequests.FirstOrDefaultAsync(x => x.Id == RequestId);
            var req_Actions = await _context.MprRequestAction.Where(x => x.RequestId == RequestId).ToListAsync();
            foreach (var ac in req_Actions) 
            {
                ac.Seen = true;
                ac.Readed = true;
                if (ac.ActionState == ActionState.Pending)
                {
                    ac.ActionState = ActionState.TakenByOther;
                }
                
            }
            request.CurrentStep = MprSteps.CreationStep;
            request.RequestState = RequestState.Canceldeld;
            request.isDone = true;
            MprRequestAction action = new MprRequestAction();
            action.ActionNote =  Note;
            action.RequestId =  RequestId;
            action.ActionState =  ActionState.Done;
            action.ActionsType = StepActions.Cancel;
            action.Step =  MprSteps.CreationStep;
            action.CreatedBy = currentUser.Id;
            action.CreatedOn = DateTime.Now;
            action.BaseId = request.BaseId;
            action.SectionId = request.SectionId;
            action.ForceId = request.ForceId;
            action.Seen = true;
            action.Readed = true;
            action.ActionResult = "Canceled By Creator";
            _context.MprRequestAction.Add(action);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SubmitRequestAsync(int ActionId, string Note, int? voteCodeId)
        {
            var action = await _context.MprRequestAction.FirstOrDefaultAsync(x => x.Id == ActionId);
            var request = await _context.MprRequests.FirstOrDefaultAsync(x => x.Id == action.RequestId);
            action.ActionNote = Note;
            action.ActionState = ActionState.Done;
            action.ActionsType = StepActions.Submit;
            action.LastModifiedBy = currentUser.Id;
            action.LastModifiedOn = DateTime.Now;
            action.Seen = true;
            action.Readed = true;
            action.ActionResult = $"Request Submited By {currentUser.FirstName} {currentUser.LastName} - {currentUser.UserName}";
            MprRequestAction NewAction = new MprRequestAction();
            NewAction.RequestId = request.Id;
            NewAction.ActionState = ActionState.Pending;
            NewAction.CreatedBy = currentUser.Id;
            NewAction.CreatedOn = DateTime.Now;
            NewAction.BaseId = request.BaseId;
            NewAction.SectionId = request.SectionId;
            NewAction.ForceId = request.ForceId;
            NewAction.Seen = false;
            NewAction.Readed = false;
            NewAction.Step = MprSteps.VoteCodeContreoller;
            request.CurrentStep = MprSteps.VoteCodeContreoller;
            request.RequestState = RequestState.Completed;
            request.Paied = false;
            request.ConfirmPaied = false;
            request.VoteCodeId = voteCodeId ?? request.VoteCodeId ;
            request.isDone = true;
            _context.MprRequestAction.Add(NewAction);
            await _context.SaveChangesAsync();
            await NotifySteppers(request);
            return await NotifyOwner(request);

        }

        public async Task<int> RejectRequestAsync(int actionID, string Note)
        {
            var action = await _context.MprRequestAction.FirstOrDefaultAsync(x => x.Id == actionID);
            var request = await _context.MprRequests.FirstOrDefaultAsync(x => x.Id == action.RequestId);
            action.ActionNote = Note;
            action.ActionState = ActionState.Done;
            action.ActionsType = StepActions.Reject;
            action.Step = MprSteps.CreationStep;
            action.LastModifiedBy = currentUser.Id;
            action.LastModifiedOn = DateTime.Now;
            action.Seen = true;
            action.Readed = true;
            action.ActionResult = $"Request Rejected By {currentUser.FirstName} {currentUser.LastName} - {currentUser.UserName}";
            request.CurrentStep = MprSteps.CreationStep;
            request.RequestState = RequestState.Rejected;
            request.isDone = true;
             await _context.SaveChangesAsync();
            return await NotifyOwner(request);
        }

        public async Task<int> EsclateRequestAsync(int ActionId, string Note)
        {
            var action = await _context.MprRequestAction.FirstOrDefaultAsync(x => x.Id == ActionId);
            var request = await _context.MprRequests.FirstOrDefaultAsync(x => x.Id == action.RequestId);
            action.ActionNote = Note;
            action.ActionState = ActionState.Done;
            action.ActionsType = StepActions.Esclate;
            action.LastModifiedBy = currentUser.Id;
            action.LastModifiedOn = DateTime.Now;
            action.Seen = true;
            action.Readed = true;
          
            action.ActionResult = $"Request Esclated By {currentUser.FirstName} {currentUser.LastName} - {currentUser.UserName}";
            MprRequestAction NewAction = new MprRequestAction();
            NewAction.RequestId = request.Id;
            NewAction.ActionState = ActionState.Pending;
            NewAction.Step = GetNextStep(request);
            NewAction.CreatedBy = currentUser.Id;
            NewAction.CreatedOn = DateTime.Now;
            NewAction.BaseId = request.BaseId;
            NewAction.SectionId = request.SectionId;
            NewAction.ForceId = request.ForceId;
            NewAction.Seen = false;
            NewAction.Readed = false;
            _context.MprRequestAction.Add(NewAction);
            request.CurrentStep = NewAction.Step;
            request.RequestState = RequestState.Pending;
            await _context.SaveChangesAsync();
            await NotifySteppers(request);
            return await NotifyOwner(request);
        }

 

        public async Task<int> RedirectRequestAsync(int ActionId, RedirectAction To, string refId,string Note, int? refIdint)
        {
            var action = await _context.MprRequestAction.FirstOrDefaultAsync(x => x.Id == ActionId);
            var request = await _context.MprRequests.FirstOrDefaultAsync(x => x.Id == action.RequestId);
            action.ActionNote = Note;
            action.ActionState = ActionState.Done;
            action.ActionsType = StepActions.Redirect;
            action.LastModifiedBy = currentUser.Id;
            action.LastModifiedOn = DateTime.Now;
            action.Seen = true;
            action.Readed = true;
            action.ActionResult = $"Request Redirected By {currentUser.FirstName} {currentUser.LastName} - {currentUser.UserName}";
            
            MprRequestAction NewAction = new MprRequestAction();
            NewAction.RequestId = request.Id;
            NewAction.ActionState = ActionState.Pending;
            NewAction.CreatedBy = currentUser.Id;
            NewAction.CreatedOn = DateTime.Now;
            NewAction.BaseId = request.BaseId;
            NewAction.SectionId = request.SectionId;
            NewAction.ForceId = request.ForceId;
            NewAction.Seen = false;
            NewAction.Readed = false;
           
           
            request.isDone = true;
            switch (To) 
            {
                case RedirectAction.ToUser:
                    NewAction.TargetUserId = refId;
                    break;
                    case RedirectAction.ToUserType:
                    var user = _userManager.Users.FirstOrDefault(x=>x.Id == refId);
                    var userType = user.UserType.Value;
                    var UserStep = mprStepTypes.GetUserStep(userType);
                    request.CurrentStep = UserStep;
                    NewAction.Step = UserStep;
                    break;
                    case RedirectAction.ToDepartment: 
                    NewAction.TargetDepartMentType = (DepartType)int.Parse(refId);
                    NewAction.TargetDepartId = refIdint;
                    break;
            }
            _context.MprRequestAction.Add(NewAction);
            await _context.SaveChangesAsync();
            await NotifySteppers(request);
            return await NotifyOwner(request);

        }
        private async Task<int> EsclateRequestAsync(MprRequest request)
        {
            MprRequestAction NewAction = new MprRequestAction();
            NewAction.RequestId = request.Id;
            NewAction.ActionState = ActionState.Pending;
            NewAction.Step = GetNextStep(request);
            NewAction.CreatedBy = currentUser.Id;
            NewAction.CreatedOn = DateTime.Now;
            NewAction.BaseId = request.BaseId;
            NewAction.SectionId = request.SectionId;
            NewAction.ForceId = request.ForceId;
            NewAction.Seen = false;
            NewAction.Readed = false;
            _context.MprRequestAction.Add(NewAction);
            request.CurrentStep = NewAction.Step;
            request.RequestState = RequestState.Pending;
            request.isDone = false;
            await _context.SaveChangesAsync();
            await NotifySteppers(request);
            return await NotifyOwner(request);
        }
        public async Task<int> EditRequestAsync(List<ItemDto> Items, int ActionId, string Note, List<UploadRequest> Attachments)
        {
            var action = await _context.MprRequestAction.FirstOrDefaultAsync(x => x.Id == ActionId);
            var request = await _context.MprRequests.Include(x=>x.RequestItems).FirstOrDefaultAsync(x => x.Id == action.RequestId);
            action.ActionNote = Note;
            action.Seen = true;
            action.ActionResult = $"Request Edited By {currentUser.FirstName} {currentUser.LastName} - {currentUser.UserName}";
            action.ActionsType = StepActions.Edit;
            action.ActionState = ActionState.Done;
            action.Readed = true;
            action.LastModifiedBy = currentUser.Id;
            action.LastModifiedOn = DateTime.Now;

            request.RequestItems.Clear();
            foreach (var item in Items)
            {
                request.RequestItems.Add(new RequestItem()
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
                    RequestId = request.Id
                });
            }
            if (Attachments.Count > 0)
            {
                var attachments = new List<MprRequestAttachments>();
                foreach (var attachment in Attachments)
                {
                    attachment.FileName = $"Q-{request.RequestRefranceCode.Replace("/", "-")}-{Guid.NewGuid()}{attachment.Extension}";
                    action.Attachments.Add(new MprRequestAttachments()
                    {
                        AttachmentType = "Qutation",
                        RequestID = request.Id,
                        FileUrl = _uploadService.UploadAsync(attachment),
                        ActionId = action.Id,
                        CreatedBy = currentUser.Id,
                        CreatedOn = DateTime.Now,
                        
                    });
                }
             
                    var saved = await AddActionAttachmentAsync(attachments, ActionId);
            }
             request = await _context.MprRequests.Include(x => x.RequestItems).FirstOrDefaultAsync(x => x.Id == action.RequestId);

            return await EsclateRequestAsync(request);
        }

        public async Task<int> SelectAttachmentAsync(int ActionId, string Note, List<string> SelectedAttachments)
        {
            var action = await _context.MprRequestAction.FirstOrDefaultAsync(x => x.Id == ActionId);
            var request = await _context.MprRequests.Include(x => x.RequestItems).FirstOrDefaultAsync(x => x.Id == action.RequestId);
            var attachments = await _context.MprRequestAttachment.Where(x => x.RequestID == action.RequestId).ToListAsync();
            action.ActionNote = Note;
            action.ActionState = ActionState.Done;
            action.ActionsType = StepActions.SelectQutation;
            action.LastModifiedBy = currentUser.Id;
            action.LastModifiedOn = DateTime.Now;
            action.Seen = true;
            action.Readed = true;
            action.ActionResult = $" {currentUser.FirstName} {currentUser.LastName} - {currentUser.UserName} Selected Qutations";
            var AttachmentList = new List<MprRequestAttachments>();
            foreach (var item in SelectedAttachments)
            {
                var exist = attachments.FirstOrDefault(x => x.FileUrl == item);
                AttachmentList.Add(new MprRequestAttachments() 
                {
                    AttachmentType = "Qutation",
                    RequestID = request.Id,
                    FileUrl = exist.FileUrl,
                    ActionId = action.Id,
                    CreatedBy = currentUser.Id,
                    CreatedOn = DateTime.Now,
                });
                exist.Selected = true;
            }
            await _context.MprRequestAttachment.AddRangeAsync(AttachmentList);
            await _context.SaveChangesAsync();
            return  await EsclateRequestAsync(request);
            
        }

        public async Task<int> SubmitPayment(int RequestId)
        {
            var request = await _context.MprRequests.Include(x=>x.RequestItems).FirstOrDefaultAsync(x => x.Id == RequestId);
            request.isDone = true;
            request.Paied = true;
            request.LastModifiedBy = currentUser.Id;
            request.LastModifiedOn = DateTime.Now;
           await _context.SaveChangesAsync();
            AddEditVcodeTransactionRequest cashRequest = new AddEditVcodeTransactionRequest()
            {
                TransactionAmount = request.RequestItems.Sum(x => x.ItemPrice),  
                Reason = $"MPR Request Deduction For Items \n {string.Join("\n", request.RequestItems.Select(x => x.ItemName).ToList())}",
                RequestId = RequestId,
                RequestRefrance = request.RequestRefranceCode,
                RequestType = "MPR",
                VoteCodeId = request.VoteCodeId,
                Transactiontype = TransactionType.Debit,
            };
              await _voteCodeService.AddEditTransaction(cashRequest);
            return 0;
        }

        public async Task<List<MprRequest>> GetRequestsByVoteCodeAsync(int voteCodeId)
        {
            return await _context.MprRequests.Where(x=>x.VoteCodeId == voteCodeId).ToListAsync();
        }

        public async Task<int> ConfirmPaied(int RequestId)
        {
            var request = await _context.MprRequests.FirstOrDefaultAsync(x => x.Id == RequestId);
            request.ConfirmPaied = true;
            request.LastModifiedBy = currentUser.Id;
            request.LastModifiedOn = DateTime.Now;
            return await _context.SaveChangesAsync();
        }
    }
}
