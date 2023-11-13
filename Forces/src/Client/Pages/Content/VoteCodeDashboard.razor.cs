using Forces.Application.Enums;
using Forces.Application.Responses.VoteCodes;
using Forces.Client.Infrastructure.Managers.VoteCodes;
using Forces.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Forces.Client.Pages.Content
{
    public partial class VoteCodeDashboard
    {
        [Inject] private IVoteCodesManager _voteCodesmanager { get; set; }
        public List<VoteCodeResponse> voteCodesList { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private bool _loaded;
        private UserType userType;
        private ClaimsPrincipal _currentUser;
        public int PrimaryCount { get; set; } = 0;
        public int SecoundryCount { get; set; } = 0;
        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();

            HubConnection = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri(ApplicationConstants.SignalR.HubUrl))
            .Build();
            HubConnection.On(ApplicationConstants.SignalR.ReceiveUpdateDashboard, async () =>
            {
                await LoadDataAsync();
                if (userType == UserType.VoteHolder)
                {
                    await GetVoteCodes();
                }
                StateHasChanged();
            });
            await HubConnection.StartAsync();
            if (userType == UserType.VoteHolder)
            {
                await GetVoteCodes();
            }
            _loaded = true;
        }
        private void CodeDetails(int CodeID)
        {
            _navigationManager.NavigateTo($"/vcodedashboard/{CodeID}");
        }
        private Color CardColor(bool isPrimary)
        {
            if (isPrimary)
            {
                return Color.Primary;
            }
            return Color.Secondary;
        }
        private async Task GetVoteCodes()
        {
            var response = await _voteCodesmanager.GetAllByCurrentUser();
            if (response.Succeeded)
            {
                voteCodesList = response.Data.OrderByDescending(x => x.IsPrimery).ToList();
                PrimaryCount = response.Data.Where(x => x.IsPrimery).Count();
                SecoundryCount = response.Data.Where(x => !x.IsPrimery).Count();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task LoadDataAsync()
        {
            var userTypeResponse = await _userManager.GetUserType();
            userType = userTypeResponse.Data;
        }
    }
}
