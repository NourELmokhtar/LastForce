using Blazored.FluentValidation;
using FluentValidation;
using Forces.Application.Features.BaseSections.Queries.GetAll;
using Forces.Application.Features.InventoryItem.Queries.GetAll;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Application.Features.InventoryItem.Queries.GetAll;
using Forces.Application.Features.Room.Queries.GetAll;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.BasicInformation.BaseSections;
using Forces.Client.Infrastructure.Managers.BasicInformation.Forces;
using Forces.Client.Infrastructure.Managers.InventoryItem;
using Forces.Client.Infrastructure.Managers.InventoryItem;
using Forces.Client.Infrastructure.Managers.InventoryItem;
using Forces.Client.Infrastructure.Managers.Room;
using Forces.Shared.Constants.Application;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Security.Claims;
using Forces.Client.Infrastructure.Managers.BasicInformation.Bases;
using Forces.Application.Features.Bases.Queries.GetAll;
using Forces.Client.Pages.Inventory;
using Forces.Application.Features.InventoryInventoryItem.Commands.AddEdit;
using Forces.Application.Features.Inventory.Queries.GetAll;
using Forces.Client.Infrastructure.Managers.Inventory;
using Forces.Application.Features.MeasureUnits.Queries.GetAll;
using Forces.Client.Infrastructure.Managers.Items.MeasureUnits;
using Forces.Client.Pages.Items;
using Forces.Application.Responses.VoteCodes;
using Forces.Client.Infrastructure.Managers.VoteCodes;

namespace Forces.Client.Pages.InventoryItem
{
    public partial class AddEditInventoryItemModal
    {
        public AddEditInventoryItemModal()
        {

        }
        [Inject] private IInventoryItemManager InventoryItemManager { get; set; }
        [Inject] private IInventoryManager InventoryManager { get; set; }
        private List<GetAllInventoriesResponse> _InventoryList = new();

        private List<GetAllMeasureUnitsResponse> _UnitsList = new();
        [Inject] private IMeasureUnitsManager UnitsManager { get; set; }


        private List<GetAllForcesResponse> _ForceList = new();
        [Parameter] public AddEditInventoryItemCommand AddEditInventoryItemModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Inject] private IForceManager ForceManager { get; set; }
        private List<VoteCodeResponse> votecodeList = new();
        [Inject] IDialogService DialogService { get; set; }
        [Inject] private IVoteCodesManager _VoteCodeMnager { get; set; }


        private string SelectedInventory;

        private bool _canCreateBaseSection;
        private bool _canEditBaseSection;
        private bool _canDeleteBaseSection;
        private bool _canSearchBaseSection;
        private ClaimsPrincipal _currentUser;

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public void Cancel()
        {
            MudDialog.Cancel();
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
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }

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

        private string VoteCodestr(int Id)
        {
            var code = votecodeList.FirstOrDefault(x => x.Id == Id);
            if (code == null)
            {
                return "";
            }
            return code.VoteCode;
        }

        private async Task GetVoteCodesAsync()
        {
            var response = await _VoteCodeMnager.GetAllAsync();
            if (response.Succeeded)
            {
                votecodeList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }

        private async Task GetUnitsAsync()
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
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }

        private async Task SaveAsync()
        {
            AddEditInventoryItemModel.InventoryId = (int)converterForInventories(SelectedInventory);
            var response = await InventoryItemManager.SaveAsync(AddEditInventoryItemModel);
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
            _canCreateBaseSection = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.CreateBasesSection)).Succeeded;
            _canEditBaseSection = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.EditBasesSection)).Succeeded;
            _canDeleteBaseSection = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.DeleteBasesSection)).Succeeded;
            _canSearchBaseSection = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.SearchBasesSection)).Succeeded;

            await LoadDataAsync();

            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        Func<int, string> converter()
        {
            return p => $"{_ForceList.FirstOrDefault(x => x.Id == p).ForceName} | {_ForceList.FirstOrDefault(x => x.Id == p).ForceCode}";
        }
        private int? converterForInventories(string ss)
        {
            return _InventoryList.FirstOrDefault(s => s.Name == ss).Id;
        }


        private async Task LoadDataAsync()
        {
            await GetInventoriesAsync();
            await GetForcesAsync();
            await GetUnitsAsync();
            await GetVoteCodesAsync();
            await Task.CompletedTask;
        }
    }

}
