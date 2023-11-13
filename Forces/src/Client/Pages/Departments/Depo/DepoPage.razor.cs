using Forces.Application.Features.DepoDepartment.Commands.AddEdit;
using Forces.Application.Features.DepoDepartment.Queries.GetAll;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.Departments.Depo;
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


namespace Forces.Client.Pages.Departments.Depo
{
    public partial class DepoPage
    {
        [Inject] private IDepoManager DepoManager { get; set; }
        [Inject] private IForceManager ForceManager { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private List<GetAllDepoDepartmentsResponse> _DepoList = new();
        private GetAllDepoDepartmentsResponse _Depo = new();
        private List<GetAllForcesResponse> _ForcesList = new();
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreate;
        private bool _canEdit;
        private bool _canDelete;
        private bool _canSearch;
        private bool _loaded;
        private bool _IsSuberAdmin;
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreate = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DepoManagement.CreateDepartments)).Succeeded;
            _canEdit = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DepoManagement.EditDepartments)).Succeeded;
            _canDelete = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DepoManagement.DeleteDepartments)).Succeeded;
            _canSearch = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DepoManagement.SearchDepartments)).Succeeded;
            await GetUserTypeAsync();
            await GetForcesAsync();
            await GetDeposAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private async Task GetDeposAsync()
        {
            var response = await DepoManager.GetAllAsync();
            if (response.Succeeded)
            {
                _DepoList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task GetUserTypeAsync()
        {
            var response = await _userManager.GetUserType();
            if (response.Succeeded)
            {
                _IsSuberAdmin = response.Data == Application.Enums.UserType.SuperAdmin;

            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task GetForcesAsync()
        {
            var response = await ForceManager.GetAllAsync();
            if (response.Succeeded)
            {
                _ForcesList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private string GetForceName(int ForceId)
        {
            var force = _ForcesList.FirstOrDefault(x => x.Id == ForceId);
            return $"{force.ForceName} | {force.ForceCode}";
        }
        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _Depo = _DepoList.FirstOrDefault(c => c.Id == id);
                if (_Depo != null)
                {
                    parameters.Add(nameof(AddEditDepoDepartmentModal.AddEditDepoModel), new AddEditDepoCommand
                    {
                        Id = _Depo.Id,
                        Name = _Depo.Name,
                        ForceID = _Depo.ForceID,
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditDepoDepartmentModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }
        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Are You Sure To Delete This Department?"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await DepoManager.DeleteAsync(id);
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
            _Depo = new GetAllDepoDepartmentsResponse();
            await GetDeposAsync();
        }
        private bool Search(GetAllDepoDepartmentsResponse depo)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (depo.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
