using DevExpress.XtraRichEdit.Model;
using Forces.Application.Features.BaseSections.Queries.GetAll;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Application.Features.Inventory.Queries.GetAll;
using Forces.Application.Features.InventoryInventoryItem.Commands.AddEdit;
using Forces.Application.Features.InventoryItem.Queries.GetAll;
using Forces.Application.Features.InventoryItemBridge.Commands.AddEdit;
using Forces.Application.Features.InventoryItemBridge.Queries.GetAll;
using Forces.Application.Interfaces.Repositories;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.BasicInformation.BaseSections;
using Forces.Client.Infrastructure.Managers.BasicInformation.Forces;
using Forces.Client.Infrastructure.Managers.InventoryItem;
using Forces.Client.Infrastructure.Managers.InventoryItemBridge;
using Forces.Client.Pages.InventoryItem;
using Forces.Shared.Constants.Application;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Security.Claims;

namespace Forces.Client.Pages.InventryItemBridge
{
    public partial class MyInventoryItemBridge
    {
        [Inject] private IInventoryItemManager InventoryItemManager { get; set; }
        [Inject] private IInventoryItemBridgeManager InventoryItemBridgeManager { get; set; }
        [Inject] private IForceManager ForceManager { get; set; }
        [Inject] private IBaseSectionManager BaseSectionManager { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private readonly IUnitOfWork<int> _unitOfWork;

        private List<GetAllInventoriesResponse> _InventoryList = new();
        private List<GetAllInventoryItemBridgeResponse> _InventoryItemBridgeList = new();
        private List<GetAllBasesSectionsQueryResponse> _BaseSectionList = new();
        private GetAllInventoryItemBridgeResponse _InventoryItemBridge = new();
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;
        private ClaimsPrincipal _currentUser;
        private bool _canCreateInventoryItemBridge;
        private bool _canEditInventoryItemBridge;
        private bool _canDeleteInventoryItemBridge;
        private bool _canSearchInventoryItemBridge;
        private bool _loaded;
        private List<GetAllForcesResponse> _ForceList = new();
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateInventoryItemBridge = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.InventoryItemsBridge.Create)).Succeeded;
            _canEditInventoryItemBridge = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.InventoryItemsBridge.Edit)).Succeeded;
            _canDeleteInventoryItemBridge = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.InventoryItemsBridge.Delete)).Succeeded;
            _canSearchInventoryItemBridge = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.InventoryItemsBridge.Search)).Succeeded;
            await GetInventoryItemsBridgeAsync();

            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }


        private async Task GetInventoryItemsBridgeAsync()
        {
            var response = await InventoryItemBridgeManager.GetAllAsync();
            if (response.Succeeded)
            {
                _InventoryItemBridgeList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private TableGroupDefinition<GetAllInventoryItemsResponse> _groupDefinition = new()
        {
            GroupName = "Force",
            Indentation = true,
            Expandable = true,
            Selector = (e) => e.ItemName
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
                _InventoryItemBridge = _InventoryItemBridgeList.FirstOrDefault(c => c.Id == id);
                if (_InventoryItemBridge != null)
                {
                    parameters.Add(nameof(AddEditInventoryItemBridgeModal.AddEditInventoryItemBridgeModel), new AddEditInventoryItemBridgeCommand
                    {
                        InventoryId = _unitOfWork.Repository<Application.Models.Inventory>().GetAllAsync().Result.FirstOrDefault(x=>x.Name==_InventoryItemBridge.InventoryName).Id,
                        InventoryItemId= _unitOfWork.Repository<Application.Models.InventoryItem>().GetAllAsync().Result.FirstOrDefault(x => x.ItemName == _InventoryItemBridge.InventoryName).Id,
                        DateOfEnter = _InventoryItemBridge.DateOfEnter,

                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditInventoryItemBridgeModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
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
                var response = await InventoryItemBridgeManager.DeleteAsync(id);
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
            _InventoryItemBridgeList = new List<GetAllInventoryItemBridgeResponse>();
            await GetInventoryItemsBridgeAsync();
        }
        private bool Search(GetAllInventoryItemBridgeResponse Base)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (Base.InventoryItemName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            if (Base.InventoryItemName.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
