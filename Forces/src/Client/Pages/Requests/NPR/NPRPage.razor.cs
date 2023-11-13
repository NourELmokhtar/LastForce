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
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;

namespace Forces.Client.Pages.Requests.NPR
{
    public partial class NPRPage
    {
        public List<GetAllMPRResponse> RequestsList { get; set; } = new();
        private GetAllMPRResponse request { get; set; } = new();
        [Inject] public INprManager nprManager { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private ClaimsPrincipal _authenticationStateProviderUser;
        private UserType CurrentUserType { get; set; }
        private UserResponse CurrentUser { get; set; }
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;
        private bool _loaded;
        private bool _canCreateMPR;
        private void AddNew()
        {
            _navigationManager.NavigateTo("/Requests/MPR/New");
        }
        protected override async Task OnInitializedAsync()
        {
            await LoadUserData();
            await LoadData();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
            _canCreateMPR = (await _authorizationService.AuthorizeAsync(_authenticationStateProviderUser, Permissions.MPR.Create)).Succeeded;
        }
        private async Task LoadUserData()
        {
            var uType = await _userManager.GetUserType();
            CurrentUserType = uType.Data;
            var authenticatedUser = await _authenticationManager.CurrentUser();
            var CurrentUserResponse = await _userManager.GetAsync(authenticatedUser.FindFirst(ClaimTypes.NameIdentifier).Value);
            CurrentUser = CurrentUserResponse.Data;
            _authenticationStateProviderUser = await _stateProvider.GetAuthenticationStateProviderUserAsync();
        }
        private void PreviewRequest(int ID)
        {
            _navigationManager.NavigateTo($"/Requests/MPR/{ID}");
        }
        private async Task LoadData()
        {
            await GetRequests();
        }
        private GetRequestsBySpecificationsRequest generateUserSesfications()
        {
            var spes = new GetRequestsBySpecificationsRequest();
            spes.BaseId = CurrentUser.BaseId;
            spes.ForceId = CurrentUser.ForceId;
            spes.BaseSectionId = CurrentUser.BaseSectionId;
            spes.TargetDeparmentType = CurrentUser.DepartType;
            spes.TargetDepartmentId = CurrentUser.DepartId;
            spes.userType = CurrentUserType;
            return spes;
        }
        private async Task GetRequests()
        {

            if (CurrentUserType == UserType.Regular)
            {
                var response = await nprManager.GetAllRequestsByUser();
                if (response.Succeeded)
                {
                    RequestsList = response.Data;
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, MudBlazor.Severity.Error);
                    }
                }
            }
            else if (CurrentUserType == UserType.OCLogAdmin || CurrentUserType == UserType.OCLog)
            {
                var response = await nprManager.GetAllRequestsToLog();
                if (response.Succeeded)
                {
                    RequestsList = response.Data;
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, MudBlazor.Severity.Error);
                    }
                }
            }
            else
            {
                var response = await nprManager.GetAllRequestsBySpecifications(generateUserSesfications());
                if (response.Succeeded)
                {
                    RequestsList = response.Data;
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
        private bool Search(GetAllMPRResponse request)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (request.ItemCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (request.ItemName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (request.ItemNameAR?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (request.ItemCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (request.ItemDescription?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (request.ItemNSN?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
        public string GetCurentActionState(RequestSteps Step, RequestState requestState)
        {
            var CurrentUserStep = GetUserStep.GetStepByUserType(CurrentUserType);
            if (Step == CurrentUserStep && requestState == RequestState.Pending)
            {
                return "Need Action";
            }
            else
            {
                return "Done";
            }
        }
    }
}
