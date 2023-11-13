using Forces.Application.Features.Bases.Commands.AddEdit;
using Forces.Application.Features.Bases.Queries.GetAll;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.BasicInformation.Bases;
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
    public partial class BasesPage
    {
        [Inject] private IBaseManager baseManager { get; set; }
        [Inject] private IForceManager ForceManager { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private List<GetAllBasesResponse> _BasesList = new();
        private GetAllBasesResponse _base = new();
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;
        private ClaimsPrincipal _currentUser;
        private bool _canCreateBase;
        private bool _canEditBase;
        private bool _canDeleteBase;
        private bool _canSearchBase;
        private bool _loaded;
        private List<GetAllForcesResponse> _ForceList = new();
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.CreateBases)).Succeeded;
            _canEditBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.EditBases)).Succeeded;
            _canDeleteBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.DeleteBases)).Succeeded;
            _canSearchBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.SearchBases)).Succeeded;

            await GetForcesAsync();
            await GetBasesAsync();

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
        private async Task GetBasesAsync()
        {
            var response = await baseManager.GetAllAsync();
            if (response.Succeeded)
            {
                _BasesList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private TableGroupDefinition<GetAllBasesResponse> _groupDefinition = new()
        {
            GroupName = "Force",
            Indentation = true,
            Expandable = true,
            Selector = (e) => e.ForceId
        };
        private string ForceNameAndCode(int id)
        {
            var force = _ForceList.FirstOrDefault(x => x.Id == id);
            return $"{force.ForceName} | {force.ForceCode}";
        }
        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _base = _BasesList.FirstOrDefault(c => c.Id == id);
                if (_base != null)
                {
                    parameters.Add(nameof(AddEditBaseModal.AddEditBaseModel), new AddEditBaseCommand
                    {
                        Id = _base.Id,
                        BaseName = _base.BaseName,
                        BaseCode = _base.BaseCode,
                        ForceId = _base.ForceId
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditBaseModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }
        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Are You Sure To Delete This Base?"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await baseManager.DeleteAsync(id);
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
            _base = new GetAllBasesResponse();
            await GetBasesAsync();
        }
        private bool Search(GetAllBasesResponse Base)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (Base.BaseName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (Base.BaseCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (Base.Force.ForceName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (Base.Force.ForceCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
