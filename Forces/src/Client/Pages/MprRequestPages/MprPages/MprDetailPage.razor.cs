using Forces.Application.Enums;
using Forces.Application.Features.DepoDepartment.Queries.GetAll;
using Forces.Application.Features.HQDepartment.Queries.GetAll;
using Forces.Application.Helper;
using Forces.Application.Requests.Requests;
using Forces.Application.Responses.Identity;
using Forces.Application.Responses.Requets.MPR;
using Forces.Application.Responses.VoteCodes;
using Forces.Client.Infrastructure.Managers.Departments.Depo;
using Forces.Client.Infrastructure.Managers.Departments.HQ;
using Forces.Client.Infrastructure.Managers.Requests.NPR;
using Forces.Client.Infrastructure.Managers.VoteCodes;
using Forces.Client.Pages.Items;
using Forces.Shared.Constants.Application;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Security.Claims;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.MPR;
using Forces.Application.Features.MprRequest.Dto.Response;
using Forces.Application.Features.MprRequest.Command;
using Forces.Application.Features.MprRequest.Command.Actions;
using DevExpress.DataAccess.Native.Web;
using Forces.Shared.Wrapper;
using Forces.Application.Features.Forces.Commands.AddEdit;
using Forces.Application.Models;
using Forces.Client.Pages.BasicInformations;
using Color = MudBlazor.Color;

namespace Forces.Client.Pages.MprRequestPages.MprPages
{
    public partial class MprDetailPage
    {
        private GetMprResponse request { get; set; }
        [Inject] public IMprRequestManager nprManager { get; set; }
        [Inject] public IDepoManager depoManager { get; set; }
        [Inject] public IHQManager HQmanager { get; set; }
        [Inject] public IVoteCodesManager voteCodeManager { get; set; }
        private UserType CurrentUserType { get; set; }
        private IList<IBrowserFile> files = new List<IBrowserFile>();
        private UserResponse CurrentUser { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        public AddMprActionCommand ActionModel { get; set; } = new();
        private List<StepActions> AvilableActions { get; set; } = new();
        private bool _loaded = false;
        [Parameter] public string requestId { get; set; }
        public List<GetAllDepoDepartmentsResponse> DepoList { get; set; } = new();
        public List<GetAllHQDepartmentsResponse> HqList { get; set; } = new();
        public List<VoteCodeResponse> VoteCodesList { get; set; } = new();
        private VoteCodeResponse SelectedVoteCode = new();
        private int AttachmentCounter = 0;
        private Dictionary<string, object> OpenNewTab = new Dictionary<string, object>();
        private MprSteps CurrentUserStep = MprSteps.CreationStep;
        private int CurrentActionId = 0;
        private bool needAction = false;
        private DepartType? departType;
        private int? SelectedDepartId = 0;
        private HashSet<string>SelectedAttachment = new();
        public RedirectActionCommand RedirectModel { get; set; } = new();
        public EditActionCommand EditModel { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(1);
            OpenNewTab.Add("target", "_blank");
            OpenNewTab.Add("rel", "noopener noreferrer");
            await LoadUserData();
            await GetRequest();
             GetAvilableAction();
            await GetHq();
            await GetDepo();
            await GetVoteCodes();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
            CurrentUserStep = mprStep.GetUserStep(CurrentUserType);
            _items = new List<BreadcrumbItem>
                                        {
                                            new BreadcrumbItem("Home", href: "/"),
                                            new BreadcrumbItem("MPR", href: "/Requests/MPR"),
                                            new BreadcrumbItem(request.RefrenceId, href: null, disabled: true)
                                        };
            _loaded = true;
            await InvokeAsync(StateHasChanged);

        }
        protected async override Task OnParametersSetAsync()
        {
            await GetRequest();
            StateHasChanged();
           


        }
        private async Task LoadUserData()
        {
            var uType = await _userManager.GetUserType();
            CurrentUserType = uType.Data;
            var authenticatedUser = await _authenticationManager.CurrentUser();
            var CurrentUserResponse = await _userManager.GetAsync(authenticatedUser.FindFirst(ClaimTypes.NameIdentifier).Value);
            CurrentUser = CurrentUserResponse.Data;

        }
        private async Task GetRequest()
        {
            EditModel.Attachments = new List<Application.Requests.UploadRequest>();
           var defaultid = 0;
            if (int.TryParse(requestId, out defaultid))
            {
                var response = await nprManager.GetAllRequestsById(int.Parse(requestId));
                if (response.Succeeded)
                {
                    request = response.Data;
                    ActionModel.RequestId = request.Id;
                    if (request.Actions.Any(x=>x.ActionStep == mprStep.GetUserStep(CurrentUserType) && x.ActionState == ActionState.Pending)) 
                    {
                        needAction = true;
                        CurrentActionId = request.Actions.FirstOrDefault(x => x.ActionStep == mprStep.GetUserStep(CurrentUserType) && x.ActionState == ActionState.Pending).Id;
                    }
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, MudBlazor.Severity.Error);
                    }
                }
            }

        }
        private DepartType GetSelectedDepartType(string dep)
        {
            DepartType depart = DepartType.Depot;
            if (int.TryParse(dep, out var val))
            {
                return (DepartType)val;
            }
            else
            {
                return depart;
            }
        }
        public async Task RejectActionRequest()
        {
            RejectActionCommand command = new RejectActionCommand();
            command.ActionId = CurrentActionId;
            command.Note = ActionModel.ActionNote;
            var Response = await nprManager.RejectActionAsync(command);
            if (Response.Succeeded)
            {
                _snackBar.Add("Action Success!", MudBlazor.Severity.Success);
                await GetRequest();
                await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifySteppers);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifyOwner);
                StateHasChanged();
            }
            else
            {
                foreach (var message in Response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }
        public async Task CancelActionRequest()
        {
            CancelActionCommand command = new CancelActionCommand();
            command.RequestId = request.Id;
            command.Reason = ActionModel.ActionNote;
            var Response = await nprManager.CancelActionAsync(command);
            if (Response.Succeeded)
            {
                _snackBar.Add("Action Success!", MudBlazor.Severity.Success);
                await GetRequest();
                await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifySteppers);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifyOwner);
                StateHasChanged();
            }
            else
            {
                foreach (var message in Response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }
        public async Task RedirectActionRequest()
        {
            RedirectModel.Note = ActionModel.ActionNote;
            RedirectModel.ActionId = CurrentActionId;
            var Response = await nprManager.RedirectActionAsync(RedirectModel);
            if (Response.Succeeded)
            {
                _snackBar.Add("Action Success!", MudBlazor.Severity.Success);
                await GetRequest();
                await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifySteppers);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifyOwner);
                StateHasChanged();
            }
            else
            {
                foreach (var message in Response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }
        public async Task EsclateActionRequest()
        {
            SclateActionCommand command = new SclateActionCommand();
            command.ActionId = CurrentActionId;
            command.Note = ActionModel.ActionNote;
            var Response = await nprManager.EsclateActionAsync(command);
            if (Response.Succeeded)
            {
                _snackBar.Add("Action Success!", MudBlazor.Severity.Success);
                await GetRequest();
                await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifySteppers);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifyOwner);
                StateHasChanged();
            }
            else
            {
                foreach (var message in Response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }
        public async Task SubmitActionRequest()
        {
            SubmitActionCommand command = new SubmitActionCommand();
            command.ActionId = CurrentActionId;
            command.Note = ActionModel.ActionNote;
            command.VodeCodeId = ActionModel.VoteCodeId;
            var Response = await nprManager.SubmitActionAsync(command);
            if (Response.Succeeded)
            {
                _snackBar.Add("Action Success!", MudBlazor.Severity.Success);
                await GetRequest();
                await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifySteppers);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifyOwner);
                StateHasChanged();
            }
            else
            {
                foreach (var message in Response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }
        public async Task SelectQutationActionRequest()
        {
            SelectQutationActionCommand command = new SelectQutationActionCommand();
            command.ActionId = CurrentActionId;
            command.Note = ActionModel.ActionNote;
            command.SelectedAttachments = SelectedAttachment.ToList();
            var Response = await nprManager.SelectQutationActionAsync(command);
            if (Response.Succeeded)
            {
                _snackBar.Add("Action Success!", MudBlazor.Severity.Success);
                await GetRequest();
                await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifySteppers);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifyOwner);
                StateHasChanged();
            }
            else
            {
                foreach (var message in Response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }
        public async Task EditActionRequest()
        {

            EditActionCommand command = new EditActionCommand();
            command.ActionID = CurrentActionId;
            command.Items = request.Items.Select(x=> new Application.Features.MprRequest.Dto.Request.ItemDto 
            {
            ItemArName = x.ItemNameAR,
            ItemCode = x.ItemCode,
            ItemId = x.ItemId,
            ItemName = x.ItemName,
            ItemNSN = x.ItemNSN,
            ItemPrice = x.ItemPrice,
            ItemQty = x.ItemQTY,
       Unit  = x.ItemUnit,
       VotecodeId = x.VotecodeId,
            }).ToList();
           await UploadAttachmentAsync();
            command.Attachments = EditModel.Attachments;
            
            command.Note = ActionModel.ActionNote;
            var Response = await nprManager.EditActionAsync(command);
            if (Response.Succeeded)
            {
                _snackBar.Add("Action Success!", MudBlazor.Severity.Success);
                await GetRequest();
                await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifySteppers);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifyOwner);
                StateHasChanged();
            }
            else
            {
                foreach (var message in Response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }
        private async Task ConfirmTransaction()
        {
            string Content = _localizer["You Are Going To Confirm Transaction"];
            var parameters = new DialogParameters
            {
                {"ContentText", Content}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.ConfirmDialog>(_localizer["Confirm"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                
                var response = await nprManager.SubmitPaymentAsync(request.Id);
                if (response.Succeeded)
                {
                    _snackBar.Add("Action Success!", MudBlazor.Severity.Success);
                    await GetRequest();
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifySteppers);
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifyOwner);
                    StateHasChanged();
                }
                else
                {

                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
        private async Task ConfirmPaiedTransaction()
        {
            string Content = _localizer["You Are Going To Confirm Transaction payment"];
            var parameters = new DialogParameters
            {
                {"ContentText", Content}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.ConfirmDialog>(_localizer["Confirm"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {

                var response = await nprManager.ConfirmPaymentAsync(request.Id);
                if (response.Succeeded)
                {
                    _snackBar.Add("Action Success!", MudBlazor.Severity.Success);
                    await GetRequest();
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifySteppers);
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifyOwner);
                    StateHasChanged();
                }
                else
                {

                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
        private async Task UploadAttachmentAsync()
        {
            EditModel.Attachments = new List<Application.Requests.UploadRequest>();

            foreach (var _file in files)
            {
                Application.Requests.UploadRequest att = new Application.Requests.UploadRequest();
                var buffer = new byte[_file.Size];
                var extension = Path.GetExtension(_file.Name);
                var format = "application/octet-stream";

                await _file.OpenReadStream(_file.Size).ReadAsync(buffer);
                att =new Application.Requests.UploadRequest(){ Data = buffer, FileName = _file.Name, UploadType = Application.Enums.UploadType.Qoutaions, Extension = extension };

                EditModel.Attachments.Add(att);
            }
        }
        private void SetActionId(int id)
        {
            //ActionModel.RequestActionId = id;
            //if (CurrentUserStep == RequestSteps.DepartmentStep && CurrentUser.DepartId != null)
            //{
            //    ActionModel.DepartType = CurrentUser.DepartType;
            //}
            //if (CurrentUserType != UserType.Commander)
            //{
            //    ActionModel.ActionsType = ActionsType.Escalate;
            //}
            //SelectedVoteCode.VoteCode = request.VoteCode;
        }
        private void UploadFiles(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles())
            {
                files.Add(file);
                EditModel.Attachments.Add(new Application.Requests.UploadRequest());
            }

        }
        private void removeItem(IBrowserFile file)
        {
            files.Remove(file);
            
            ActionModel.Attachments.Remove(ActionModel.Attachments.First());
        }
        private void CleareAttachment()
        {
            files.Clear();
            ActionModel.Attachments.Clear();
        }
        private Color RequestStateColor()
        {
            switch (request.RequestState)
            {
                case RequestState.Pending:
                    return Color.Warning;
                case RequestState.Completed:
                    return Color.Success;
                case RequestState.Rejected:
                    return Color.Error;
                case RequestState.Canceldeld:
                    return Color.Dark;
                default:
                    return Color.Dark;
            }
        }
        private async Task SelectVoteCode()
        {
            var dialog = DialogService.Show<SelectVoteCodesModal>();
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                ActionModel.VoteCodeId = int.Parse(result.Data.ToString());
                SelectedVoteCode = VoteCodesList.FirstOrDefault(x => x.Id == ActionModel.VoteCodeId);
            }
        }
        private List<BreadcrumbItem> _items = new();
        private void GetAvilableAction()
        {
           AvilableActions = (mprStep.StepsActions[mprStep.GetUserStep(CurrentUserType)]).ToList();
            ActionModel.Action = AvilableActions.FirstOrDefault();

        }
        private async Task GetVoteCodes()
        {
            var response = await voteCodeManager.GetAllAsync();
            if (response.Succeeded)
            {
                VoteCodesList = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }

        }
        private async Task GetDepo()
        {
            var response = await depoManager.GetAllDepoAsync();
            if (response.Succeeded)
            {
                DepoList = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }

        }
        private async Task GetHq()
        {
            var response = await HQmanager.GetAllHqAsync();
            if (response.Succeeded)
            {
                HqList = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }

        }
        private List<GetMprResponse> GetDatasource()
        {
            List<GetMprResponse> list = new List<GetMprResponse>();
            request.Actions.RemoveAll(x => x.Id == 0);
            if (!request.Actions.Any(x=>x.ActionStep == MprSteps.CreationStep))
            {
                request.Actions.Add(new Application.Features.MprRequest.Dto.Response.RequestActions()
                {
                    ActionStep = MprSteps.CreationStep,
                    ActionDate = request.CreationDate,
                    UserName = request.UserName,
                    FullName= request.Name,
                    Rank = request.Rank,
                    strRank = request.strRank,
                    
                });
            }

            list.Add(request);
            return list;
        }
        private async Task InvokeEditModal()
        {
            var parameters = new DialogParameters();

            parameters.Add(nameof(EditMprRequestModal.Items), new List<ItemDto>(request.Items) ) ; 
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<EditMprRequestModal>( _localizer["Edit Request"] , parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                //var ResultData = (List<ItemDto>)result.Data;
                request.Items = (List<ItemDto>)result.Data;

            }
        }
        private async Task Submitrequest()
        {


            switch (ActionModel.Action)
            {
                case StepActions.Submit:

                    break;
                case StepActions.Esclate:
                    await EsclateActionRequest();
                    break;
                case StepActions.Reject:
                    await RejectActionRequest();
                    break;
                case StepActions.Redirect:
                    await RedirectActionRequest();
                    break;
                case StepActions.SelectQutation:
                    await SelectQutationActionRequest();
                    break;
                case StepActions.Edit:
                    await EditActionRequest();
                    break;
                case StepActions.Cancel:
                    await CancelActionRequest();
                    break;
                default:
                    break;
            }
             
        }
    }
}
