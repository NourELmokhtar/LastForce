using Forces.Application.Features.Bases.Commands.AddEdit;
using Forces.Application.Features.Bases.Queries.GetAll;
using Forces.Application.Features.BaseSections.Queries.GetAll;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Application.Features.Inventory.Commands.AddEdit;
using Forces.Application.Features.Inventory.Queries.GetAll;
using Forces.Application.Interfaces.Repositories;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.BasicInformation.Bases;
using Forces.Client.Infrastructure.Managers.BasicInformation.BaseSections;
using Forces.Client.Infrastructure.Managers.BasicInformation.Forces;
using Forces.Client.Infrastructure.Managers.Inventory;
using Forces.Client.Pages.BasicInformations;
using Forces.Shared.Constants.Application;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Security.Claims;

namespace Forces.Client.Pages.Inventory
{
    public partial class MyInventoryPage
    {
        [Inject] private IInventoryManager InventoryManager { get; set; }
        [Inject] private IForceManager ForceManager { get; set; }
        [Inject] private IBaseSectionManager BaseSectionManager { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private readonly IUnitOfWork<int> _unitOfWork;
        private List<GetAllInventoriesResponse> _InventoriesList = new();
        private List<GetAllBasesSectionsQueryResponse> _BaseSectionList = new();
        private GetAllInventoriesResponse _Inventory = new();
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
            _canCreateBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.Create)).Succeeded;
            _canEditBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.Edit)).Succeeded;
            _canDeleteBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.Delete)).Succeeded;
            _canSearchBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Inventory.Search)).Succeeded;
            await GetBasesAsync();
            await GetForcesAsync();
            await GetInventoriesAsync();

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
        private async Task GetInventoriesAsync()
        {
            var response = await InventoryManager.GetAllAsync();
            if (response.Succeeded)
            {
                _InventoriesList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private TableGroupDefinition<GetAllInventoriesResponse> _groupDefinition = new()
        {
            GroupName = "Force",
            Indentation = true,
            Expandable = true,
            Selector = (e) => e.Name
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
                _Inventory = _InventoriesList.FirstOrDefault(c => c.Id == id);
                if (_Inventory != null)
                {
                    parameters.Add(nameof(AddEditInventoryModal.AddEditInventoryModel), new AddEditInventoryCommand
                    {
                        Id = _Inventory.Id,
                        Name = _Inventory.Name,
                        BaseSectionId = _unitOfWork.Repository<Application.Models.BasesSections>().GetAllAsync().Result.Where(y => y.SectionName == _Inventory.BaseSectionName).FirstOrDefault().Id,
                        HouseId = _unitOfWork.Repository<Application.Models.House>().GetAllAsync().Result.Where(y => y.HouseName == _Inventory.HouseName).FirstOrDefault().Id,
                        RoomId = _unitOfWork.Repository<Application.Models.Room>().GetAllAsync().Result.Where(y => y.RoomNumber == _Inventory.RoomName).FirstOrDefault().Id,
                    }); ;
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditInventoryModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
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
                var response = await InventoryManager.DeleteAsync(id);
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
            _Inventory = new GetAllInventoriesResponse();
            await GetInventoriesAsync();
        }
        private bool Search(GetAllInventoriesResponse Base)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (Base.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            if (Base.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
