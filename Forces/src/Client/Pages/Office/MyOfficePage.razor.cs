using Forces.Application.Features.Bases.Commands.AddEdit;
using Forces.Application.Features.Bases.Queries.GetAll;
using Forces.Application.Features.BaseSections.Queries.GetAll;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Application.Features.Office.Commands.AddEdit;
using Forces.Application.Features.Office.Queries.GetAll;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.BasicInformation.BaseSections;
using Forces.Client.Infrastructure.Managers.BasicInformation.Forces;
using Forces.Client.Infrastructure.Managers.Office;
using Forces.Client.Pages.BasicInformations;
using Forces.Shared.Constants.Application;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Security.Claims;

namespace Forces.Client.Pages.Office
{
    public partial class MyOfficePage
    {
        [Inject] private IOfficeManager OfficeManager { get; set; }
        [Inject] private IForceManager ForceManager { get; set; }
        [Inject] private IBaseSectionManager BaseSectionManager { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private List<GetAllOfficeResponse> _OfficesList = new();
        private List<GetAllBasesSectionsQueryResponse> _BaseSectionList = new();
        private GetAllOfficeResponse _Office = new();
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
            _canCreateBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Office.Create)).Succeeded;
            _canEditBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Office.Edit)).Succeeded;
            _canDeleteBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Office.Delete)).Succeeded;
            _canSearchBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Office.Search)).Succeeded;
            await GetBasesAsync();
            await GetForcesAsync();
            await GetOfficesAsync();

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
            var response = await BaseSectionManager.GetAllAsync();
            if (response.Succeeded)
            {
                _BaseSectionList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }
        private async Task GetOfficesAsync()
        {
            var response = await OfficeManager.GetAllAsync();
            if (response.Succeeded)
            {
                _OfficesList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private TableGroupDefinition<GetAllOfficeResponse> _groupDefinition = new()
        {
            GroupName = "Force",
            Indentation = true,
            Expandable = true,
            Selector = (e) => e.OfficeName
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
                _Office = _OfficesList.FirstOrDefault(c => c.Id == id);
                if (_Office != null)
                {
                    parameters.Add(nameof(AddEditOfficeModal.AddEditOfficeModel), new AddEditOfficeCommand
                    {
                        Id = _Office.Id,
                        OfficeName = _Office.OfficeName,
                        BaseSectionId = _Office.BasesSectionsId,
                       
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditOfficeModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
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
                var response = await OfficeManager.DeleteAsync(id);
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
            _Office = new GetAllOfficeResponse();
            await GetOfficesAsync();
        }
        private bool Search(GetAllOfficeResponse Base)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (Base.OfficeName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            if (Base.OfficeName.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
