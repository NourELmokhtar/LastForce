using Forces.Application.Features.Bases.Commands.AddEdit;
using Forces.Application.Features.Bases.Queries.GetAll;
using Forces.Application.Features.BaseSections.Queries.GetAll;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Application.Features.Building.Commands.AddEdit;
using Forces.Application.Features.Building.Queries.GetAll;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.BasicInformation.Bases;
using Forces.Client.Infrastructure.Managers.BasicInformation.BaseSections;
using Forces.Client.Infrastructure.Managers.BasicInformation.Forces;
using Forces.Client.Infrastructure.Managers.Building;
using Forces.Client.Pages.BasicInformations;
using Forces.Shared.Constants.Application;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Security.Claims;
using Forces.Application.Interfaces.Repositories;

namespace Forces.Client.Pages.Building
{
    public partial class MyBuildingsPage
    {
        [Inject] private IBuildingManager BuildingManager { get; set; }
        [Inject] private IForceManager ForceManager { get; set; }
        [Inject] private IBaseSectionManager BaseSectionManager { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private readonly IUnitOfWork<int> _unitOfWork;
        private List<GetAllBuildingsResponse> _BuildingsList = new();
        private List<GetAllBasesSectionsQueryResponse> _BaseSectionList = new();
        private GetAllBuildingsResponse _Building = new();
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
            _canCreateBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Building.Create)).Succeeded;
            _canEditBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Building.Edit)).Succeeded;
            _canDeleteBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Building.Delete)).Succeeded;
            _canSearchBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Building.Search)).Succeeded;
            await GetBasesAsync();
            await GetForcesAsync();
            await GetBuildingsAsync();

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
        private async Task GetBuildingsAsync()
        {
            var response = await BuildingManager.GetAllAsync();
            if (response.Succeeded)
            {
                _BuildingsList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private TableGroupDefinition<GetAllBuildingsResponse> _groupDefinition = new()
        {
            GroupName = "Force",
            Indentation = true,
            Expandable = true,
            Selector = (e) => e.BuildingName
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
                _Building = _BuildingsList.FirstOrDefault(c => c.Id == id);
                if (_Building != null)
                {
                    parameters.Add(nameof(AddEditBuildingModal.AddEditBuildingModel), new AddEditBuildingCommand
                    {
                        Id = _Building.Id,
                        BuildingName = _Building.BuildingName,
                        BuildingCode = _Building.BuildingCode,
                        BaseId = _unitOfWork.Repository<Application.Models.Bases>().GetAllAsync().Result.Where(y => y.BaseName == _Building.BaseName).FirstOrDefault().Id,

                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditBuildingModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
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
                var response = await BuildingManager.DeleteAsync(id);
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
            _Building = new GetAllBuildingsResponse();
            await GetBuildingsAsync();
        }
        private bool Search(GetAllBuildingsResponse Base)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (Base.BuildingName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            if (Base.BuildingName.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
