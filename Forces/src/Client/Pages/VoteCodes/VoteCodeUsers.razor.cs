using Forces.Application.Requests.VoteCodes;
using Forces.Application.Responses.Identity;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.VoteCodes;
using Forces.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Forces.Client.Pages.VoteCodes
{
    public partial class VoteCodeUsers
    {
        [Parameter] public AddEditVoteCodeRequest AddEditModel { get; set; } = new();
        private List<UserResponse> _voteUsers = new();
        private HashSet<UserResponse> SelectedUsers = new();
        private UserResponse _user;
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Inject] private IVoteCodesManager CodeManager { get; set; }
        private string searchString = "";
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        private async Task GetAllUsersAsync()
        {
            var response = await _userManager.GetAllVoteCodeHoldersAsync(AddEditModel.Id);
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
        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private async Task LoadDataAsync()
        {
            await GetAllUsersAsync();
            if (AddEditModel.Holders.Count > 0)
            {
                foreach (var item in AddEditModel.Holders)
                {
                    SelectedUsers.Add(_voteUsers.FirstOrDefault(x => x.Id == item));
                }
            }
            await Task.CompletedTask;
        }
        private async Task SaveAsync()
        {
            AddEditModel.Holders = SelectedUsers.Select(x => x.Id).ToList();
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
        private bool Search(UserResponse user)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (user.UserName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (user.JobTitle.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
    }
}
