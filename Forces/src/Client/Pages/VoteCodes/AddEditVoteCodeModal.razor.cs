
using Blazored.FluentValidation;
using Forces.Application.Requests.VoteCodes;
using Forces.Application.Responses.Identity;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.BasicInformation.Forces;
using Forces.Client.Infrastructure.Managers.VoteCodes;
using Forces.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Forces.Client.Pages.VoteCodes
{
    public partial class AddEditVoteCodeModal
    {
        [Inject] private IVoteCodesManager CodeManager { get; set; }
        [Inject] private IForceManager ForceManager { get; set; }
        private List<UserResponse> _voteUsers = new();
        private List<GetAllForcesResponse> ForcesList = new();
        [Parameter] public AddEditVoteCodeRequest AddEditModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private bool coerceValue;
        UserResponse selectedUser = new();

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        private async Task GetAllUsersAsync()
        {
            var response = await _userManager.GetAllVoteCodeHoldersAsync(-1);
            if (response.Succeeded)
            {
                _voteUsers = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task GetAllForces()
        {
            var response = await ForceManager.GetAllAsync();
            if (response.Succeeded)
            {
                ForcesList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task SaveAsync()
        {
            AddEditModel.DfaultHolderId = selectedUser?.Id;
            var response = await CodeManager.SaveAsync(AddEditModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
        }
        private async Task<IEnumerable<UserResponse>> SearchAsync(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _voteUsers;
            }

            return _voteUsers.Where(x => x.UserName.StartsWith(value, StringComparison.InvariantCultureIgnoreCase));
        }
        private Task OnUserChange()
        {
            AddEditModel.DfaultHolderId = selectedUser.Id;
            return Task.CompletedTask;
        }
        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        Func<UserResponse, string> converter()
        {
            return p => $"{_voteUsers.FirstOrDefault(x => x.UserName == p.UserName).UserName}";
        }
        Func<int, string> Forceconverter()
        {
            return p => $"{ForcesList.FirstOrDefault(x => x.Id == p).ForceName} | {ForcesList.FirstOrDefault(x => x.Id == p).ForceCode}";
        }
        private async Task LoadDataAsync()
        {
            await GetAllUsersAsync();
            await GetAllForces();
            if (AddEditModel.Id == 0)
            {
                AddEditModel.ForceId = ForcesList.FirstOrDefault().Id;
            }
            else
            {
                if (!string.IsNullOrEmpty(AddEditModel.DfaultHolderId))
                {
                    selectedUser = _voteUsers.FirstOrDefault(x => x.Id == AddEditModel.DfaultHolderId);
                }

            }
            await Task.CompletedTask;
        }
    }
}
