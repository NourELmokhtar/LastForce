using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forces.Application.Responses.Requets.MPR;
using Forces.Client.Infrastructure.Managers.Requests.NPR;
using Forces.Application.Enums;
using Forces.Application.Requests.Requests;
using Forces.Application.Responses.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR.Client;
using Forces.Client.Extensions;
using Forces.Application.Helper;
using MudBlazor;
using Forces.Client.Infrastructure.Managers.Departments.Depo;
using Forces.Client.Infrastructure.Managers.Departments.HQ;
using Forces.Application.Features.DepoDepartment.Queries.GetAll;
using Forces.Application.Features.HQDepartment.Queries.GetAll;
using Forces.Client.Pages.Items;
using Forces.Client.Infrastructure.Managers.VoteCodes;
using Forces.Application.Responses.VoteCodes;
using Forces.Shared.Constants.Application;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;

namespace Forces.Client.Pages.Requests.NPR
{
    public partial class MPRDetail
    {
        private GetAllMPRResponse request { get; set; }
        [Inject] public INprManager nprManager { get; set; }
        [Inject] public IDepoManager depoManager { get; set; }
        [Inject] public IHQManager HQmanager { get; set; }
        [Inject] public IVoteCodesManager voteCodeManager { get; set; }
        private UserType CurrentUserType { get; set; }
        private IList<IBrowserFile> files = new List<IBrowserFile>();
        private UserResponse CurrentUser { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        public ActionRequest ActionModel { get; set; } = new();
        private List<ActionsType> AvilableActions { get; set; } = new();
        private bool _loaded = false;
        [Parameter] public string requestId { get; set; }
        public List<GetAllDepoDepartmentsResponse> DepoList { get; set; } = new();
        public List<GetAllHQDepartmentsResponse> HqList { get; set; } = new();
        public List<VoteCodeResponse> VoteCodesList { get; set; } = new();
        private VoteCodeResponse SelectedVoteCode = new();
        private int AttachmentCounter = 0;
        private Dictionary<string, object> OpenNewTab = new Dictionary<string, object>();
        private RequestSteps CurrentUserStep = RequestSteps.CreationStep;
        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(1);
            OpenNewTab.Add("target", "_blank");
            OpenNewTab.Add("rel", "noopener noreferrer");
            await LoadUserData();
            await GetRequest();
            await GetAvilableAction();
            await GetHq();
            await GetDepo();
            await GetVoteCodes();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
            CurrentUserStep = GetUserStep.GetStepByUserType(CurrentUserType);
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
            var defaultid = 0;
            if (int.TryParse(requestId, out defaultid))
            {
                var response = await nprManager.GetAllRequestsById(int.Parse(requestId));
                if (response.Succeeded)
                {
                    request = response.Data;
                    ActionModel.RequestId = request.Id;
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
        public async Task RejectRequest()
        {
            ActionModel.ActionsType = ActionsType.Reject;
            ActionModel.RequestId = request.Id;
            ActionModel.RequestActionId = request.Actions.FirstOrDefault(x => x.ActionStep == RequestSteps.VoteCodeContreoller).Id;
            var response = await nprManager.SubmitAction(ActionModel);
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
                var voteCodes = await voteCodeManager.GetAllAsync();
                var VoteCodeID = voteCodes.Data.FirstOrDefault(x => x.VoteCode == request.VoteCode).Id;
                var response = await voteCodeManager.AddEditTransaction(new Application.Requests.VoteCodes.AddEditVcodeTransactionRequest()
                {
                    RequestId = request.Id,
                    TransactionAmount = request.ItemPrice,
                    RequestRefrance = request.RefrenceId,
                    ItemNSN = request.ItemNSN,
                    Reason = $"Transaction For Request {request.RefrenceId}",
                    RequestType = "MPR",
                    Transactiontype = TransactionType.Debit,
                    VoteCodeId = VoteCodeID

                });
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
            ActionModel.Attachments = new List<Application.Requests.Requests.AttachmentRequest>();

            foreach (var _file in files)
            {
                Application.Requests.Requests.AttachmentRequest att = new Application.Requests.Requests.AttachmentRequest();
                var buffer = new byte[_file.Size];
                var extension = Path.GetExtension(_file.Name);
                var format = "application/octet-stream";

                await _file.OpenReadStream(_file.Size).ReadAsync(buffer);
                att = new Application.Requests.Requests.AttachmentRequest { Data = buffer, FileName = _file.Name, UploadType = Application.Enums.UploadType.Qoutaions, Extension = extension };

                ActionModel.Attachments.Add(att);
            }
        }
        private void SetActionId(int id)
        {
            ActionModel.RequestActionId = id;
            if (CurrentUserStep == RequestSteps.DepartmentStep && CurrentUser.DepartId != null)
            {
                ActionModel.DepartType = CurrentUser.DepartType;
            }
            if (CurrentUserType != UserType.Commander)
            {
                //  ActionModel.ActionsType = ActionsType.Escalate;
            }
            //  SelectedVoteCode.VoteCode = request.VoteCode;
        }
        private void UploadFiles(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles())
            {
                files.Add(file);
                ActionModel.Attachments.Add(new Application.Requests.Requests.AttachmentRequest());
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
        private async Task GetAvilableAction()
        {
            var response = await nprManager.GetAvilableActions();
            if (response.Succeeded)
            {
                AvilableActions = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }

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

        private async Task Submitrequest()
        {
            await UploadAttachmentAsync();
            var response = await nprManager.SubmitAction(ActionModel);
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
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }


    }
}
