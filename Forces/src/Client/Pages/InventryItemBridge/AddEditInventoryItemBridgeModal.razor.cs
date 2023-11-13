using Blazored.FluentValidation;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Application.Features.Inventory.Queries.GetAll;
using Forces.Application.Features.InventoryItem.Queries.GetAll;
using Forces.Application.Features.InventoryItemBridge.Commands.AddEdit;
using Forces.Application.Features.MeasureUnits.Queries.GetAll;
using Forces.Application.Responses.VoteCodes;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.BasicInformation.Forces;
using Forces.Client.Infrastructure.Managers.Inventory;
using Forces.Client.Infrastructure.Managers.InventoryItem;
using Forces.Client.Infrastructure.Managers.InventoryItemBridge;
using Forces.Client.Infrastructure.Managers.Items.MeasureUnits;
using Forces.Client.Infrastructure.Managers.VoteCodes;
using Forces.Shared.Constants.Application;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Security.Claims;

namespace Forces.Client.Pages.InventryItemBridge
{
    public class AddInventoryModel
    {
        public int InventoryId { get; set; }
        public int Count { get; set; } = 0;
        public List<string> SerialNumbers { get; set; } 
    }
    public partial class AddEditInventoryItemBridgeModal
    {

        public AddEditInventoryItemBridgeModal() { }
        [Inject] private IInventoryItemBridgeManager InventoryItemBridgeManager { get; set; }
        [Inject] private IInventoryItemManager InventoryItemManager { get; set; }
        [Inject] private IInventoryManager InventoryManager { get; set; }
        private List<GetAllInventoriesResponse> _InventoryList = new();
        private List<GetAllInventoryItemsResponse> _InventoryItemsList = new();
        private List<string>  _InventoryItemNames { get; set; }

        private List<GetAllMeasureUnitsResponse> _UnitsList = new();
        [Inject] private IMeasureUnitsManager UnitsManager { get; set; }


        public AddInventoryModel AddInventoryModel = new AddInventoryModel();

        [Parameter] public AddEditInventoryItemBridgeCommand AddEditInventoryItemBridgeModel { get; set; } = new();
        public int Count { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Inject] private IForceManager ForceManager { get; set; }
        private List<VoteCodeResponse> votecodeList = new();
        [Inject] IDialogService DialogService { get; set; }
        [Inject] private IVoteCodesManager _VoteCodeMnager { get; set; }


        private string SelectedInventory;

        private bool _canCreateInventoryItemBridge;
        private bool _canEditInventoryItemBridge;
        private bool _canDeleteInventoryItemBridge;
        private bool _canSearchInventoryItemBridge;
        private ClaimsPrincipal _currentUser;

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public void Cancel()
        {
            MudDialog.Cancel();
        }


        List<string> selectedItems = new List<string>();

        void ItemClicked(GetAllInventoryItemsResponse allInventoryItemsResponse) 
        {
            
        }
        

        private async Task GetInventoriesAsync()
        {
            var response = await InventoryManager.GetAllAsync();
            if (response.Succeeded)
            {
                _InventoryList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }
        
        private async Task GetInventoruItemsAsync()
        {
            var response = await InventoryItemManager.GetAllAsync();
            if (response.Succeeded)
            {
                _InventoryItemsList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }

        
        private async Task SaveAsync()
        {
            AddEditInventoryItemBridgeModel.InventoryId = (int)converterForInventories(SelectedInventory);
            var response = await InventoryItemBridgeManager.SaveAsync(AddEditInventoryItemBridgeModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], MudBlazor.Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
            await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
        }
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateInventoryItemBridge = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.InventoryItemsBridge.Create)).Succeeded;
            _canEditInventoryItemBridge = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.InventoryItemsBridge.Edit)).Succeeded;
            _canDeleteInventoryItemBridge = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.InventoryItemsBridge.Delete)).Succeeded;
            _canSearchInventoryItemBridge = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.InventoryItemsBridge.Search)).Succeeded;

            await LoadDataAsync();

            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private int? converterForInventories(string ss)
        {
            return _InventoryList.FirstOrDefault(s => s.Name == ss).Id;
        }


        private async Task LoadDataAsync()
        {
            await GetInventoruItemsAsync();
            await GetInventoriesAsync();
            await Task.CompletedTask;
        }
    }
}
