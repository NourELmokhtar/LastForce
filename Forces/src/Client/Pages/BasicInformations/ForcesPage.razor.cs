using Forces.Application.Features.Forces.Commands.AddEdit;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.BasicInformation.Forces;
using Forces.Shared.Constants.Application;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Forces.Client.Pages.BasicInformations
{
    public partial class ForcesPage
    {
        [Inject] private IForceManager ForceManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private List<GetAllForcesResponse> _ForceList = new();
        private GetAllForcesResponse _Force = new();
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateForce;
        private bool _canEditForce;
        private bool _canDeleteForce;
        private bool _canSearchForces;
        private bool _loaded;
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateForce = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.CreateForces)).Succeeded;
            _canEditForce = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.EditForces)).Succeeded;
            _canDeleteForce = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.DeleteForces)).Succeeded;
            _canSearchForces = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.SearchForces)).Succeeded;


            await GetForcesAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private async Task GetForcesAsync()
        {
            var response = await ForceManager.GetAllAsync();
            if (response.Succeeded)
            {
                _ForceList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _Force = _ForceList.FirstOrDefault(c => c.Id == id);
                if (_Force != null)
                {
                    parameters.Add(nameof(AddEditForce.AddEditForceModel), new AddEditForceCommand
                    {
                        Id = _Force.Id,
                        ForceName = _Force.ForceName,
                        ForceCode = _Force.ForceCode,
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditForce>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }
        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Are You Sure To Delete This Force?"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await ForceManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
        private async Task Reset()
        {
            _Force = new GetAllForcesResponse();
            await GetForcesAsync();
        }
        private bool Search(GetAllForcesResponse force)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (force.ForceName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (force.ForceCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
