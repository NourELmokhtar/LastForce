using Forces.Application.Enums;
using Forces.Application.Responses.VoteCodes;
using Forces.Client.Infrastructure.Managers.Dashboard;
using Forces.Client.Infrastructure.Managers.VoteCodes;
using Forces.Shared.Constants.Application;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Forces.Client.Pages.Content
{
    public partial class Dashboard
    {
        [Inject] private IDashboardManager DashboardManager { get; set; }
        [Inject] private IVoteCodesManager _voteCodesmanager { get; set; }
        public List<VoteCodeResponse> voteCodesList { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Parameter] public int ProductCount { get; set; }
        [Parameter] public int BrandCount { get; set; }
        [Parameter] public int DocumentCount { get; set; }
        [Parameter] public int DocumentTypeCount { get; set; }
        [Parameter] public int DocumentExtendedAttributeCount { get; set; }
        [Parameter] public int UserCount { get; set; }
        [Parameter] public int RoleCount { get; set; }
        [Parameter] public int ForcesCount { get; set; }
        [Parameter] public int BasesCount { get; set; }
        [Parameter] public int BaseSectionsCount { get; set; }
        [Parameter] public int DepoCount { get; set; }
        [Parameter] public int HQCount { get; set; }
        [Parameter] public int VoteCodesCount { get; set; }
        [Parameter] public int VoteCodesUsersCount { get; set; }
        [Parameter] public int ItemsCount { get; set; }
        [Parameter] public int MeasureUnitsCount { get; set; }

        private readonly string[] _dataEnterBarChartXAxisLabels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        private readonly List<ChartSeries> _dataEnterBarChartSeries = new();
        private bool _loaded;
        private UserType userType;
        private ClaimsPrincipal _currentUser;
        private bool _canViewItems;
        private bool _canViewMeasureUnits;
        private bool _canViewDepo;
        private bool _canViewHQ;
        private bool _canViewVoteCodes;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            _currentUser = await _authenticationManager.CurrentUser();
            _canViewItems = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Items.View)).Succeeded;
            _canViewMeasureUnits = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.MeasureUnits.View)).Succeeded;
            _canViewDepo = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DepoManagement.ViewDepartments)).Succeeded;
            _canViewHQ = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.HQManagement.ViewDepartments)).Succeeded;
            _canViewVoteCodes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VoteCodes.View)).Succeeded;

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
        private async Task GetVoteCodes()
        {
            var response = await _voteCodesmanager.GetAllByCurrentUser();
            if (response.Succeeded)
            {
                voteCodesList = response.Data.Where(x => x.IsPrimery).ToList();
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

            var response = await DashboardManager.GetDataAsync();
            if (response.Succeeded)
            {
                ProductCount = response.Data.ProductCount;
                BrandCount = response.Data.BrandCount;
                DocumentCount = response.Data.DocumentCount;
                DocumentTypeCount = response.Data.DocumentTypeCount;
                DocumentExtendedAttributeCount = response.Data.DocumentExtendedAttributeCount;
                UserCount = response.Data.UserCount;
                RoleCount = response.Data.RoleCount;
                ForcesCount = response.Data.ForcesCount;
                BasesCount = response.Data.BasesCount;
                BaseSectionsCount = response.Data.BasesectionsCount;
                DepoCount = response.Data.DepoCount;
                HQCount = response.Data.HQCount;
                VoteCodesCount = response.Data.VoteCodesCount;
                VoteCodesUsersCount = response.Data.VoteCodeUsersCount;
                ItemsCount = response.Data.ItemsCount;
                MeasureUnitsCount = response.Data.MeasureUnitsCount;

                foreach (var item in response.Data.DataEnterBarChart)
                {
                    _dataEnterBarChartSeries
                        .RemoveAll(x => x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
                    _dataEnterBarChartSeries.Add(new ChartSeries { Name = item.Name, Data = item.Data });
                }
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
}