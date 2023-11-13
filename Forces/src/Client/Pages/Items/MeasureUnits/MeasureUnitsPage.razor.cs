using Forces.Application.Features.MeasureUnits.Commands.AddEdit;
using Forces.Application.Features.MeasureUnits.Queries.GetAll;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.Items.MeasureUnits;
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



namespace Forces.Client.Pages.Items.MeasureUnits
{
    public partial class MeasureUnitsPage
    {
        [Inject] private IMeasureUnitsManager UnitsManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private List<GetAllMeasureUnitsResponse> _UnitsList = new();
        private GetAllMeasureUnitsResponse _Unit = new();
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
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreate = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.MeasureUnits.Create)).Succeeded;
            _canEdit = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.MeasureUnits.Edit)).Succeeded;
            _canDelete = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.MeasureUnits.Delete)).Succeeded;
            _canSearch = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.MeasureUnits.Search)).Succeeded;


            await GetAllAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private async Task GetAllAsync()
        {
            var response = await UnitsManager.GetAllAsync();
            if (response.Succeeded)
            {
                _UnitsList = response.Data.ToList();
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
                _Unit = _UnitsList.FirstOrDefault(c => c.Id == id);
                if (_Unit != null)
                {
                    parameters.Add(nameof(AddEditMeasureUnitModal.AddEditModel), new AddEditMeasureUnitsCommand
                    {
                        Id = _Unit.Id,
                        MeasureName = _Unit.Name
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditMeasureUnitModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }
        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Are You Sure To Delete This Measure Unit?"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await UnitsManager.DeleteAsync(id);
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
            _Unit = new();
            await GetAllAsync();
        }
        private bool Search(GetAllMeasureUnitsResponse unit)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (unit.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
