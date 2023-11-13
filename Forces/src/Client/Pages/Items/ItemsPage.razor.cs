using Forces.Application.Features.Items.Commands.AddEdit;
using Forces.Application.Features.Items.Queries.GetAll;
using Forces.Application.Features.MeasureUnits.Queries.GetAll;
using Forces.Application.Responses.VoteCodes;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.Items;
using Forces.Client.Infrastructure.Managers.Items.MeasureUnits;
using Forces.Client.Infrastructure.Managers.VoteCodes;
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



namespace Forces.Client.Pages.Items
{
    public partial class ItemsPage
    {
        [Inject] private IItemsManager ItemManager { get; set; }
        [Inject] private IVoteCodesManager voteCodeManager { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private List<GetAllItemsResponse> _ItemsList = new();
        private GetAllItemsResponse _item = new();
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;
        private ClaimsPrincipal _currentUser;
        private bool _canCreate;
        private bool _canEdit;
        private bool _canDelete;
        private bool _canSearch;
        private bool _canExport;
        private bool _loaded;
        private List<GetAllMeasureUnitsResponse> _UnitList = new();
        private List<VoteCodeResponse> votecodeList = new();
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreate = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Items.Create)).Succeeded;
            _canEdit = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Items.Edit)).Succeeded;
            _canDelete = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Items.Delete)).Succeeded;
            _canSearch = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Items.Search)).Succeeded;
            _canSearch = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Items.Export)).Succeeded;
            await GetVoteCodesAsync();
            await GetItemsAsync();

            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private string VoteCodeString(int Id)
        {
            return votecodeList.FirstOrDefault(x => x.Id == Id).VoteCode;
        }
        private async Task GetVoteCodesAsync()
        {
            var response = await voteCodeManager.GetAllAsync();
            if (response.Succeeded)
            {
                votecodeList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task GetItemsAsync()
        {
            var response = await ItemManager.GetAllAsync();
            if (response.Succeeded)
            {
                _ItemsList = response.Data.ToList();
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
                _item = _ItemsList.FirstOrDefault(c => c.Id == id);
                if (_item != null)
                {
                    parameters.Add(nameof(AddEditItemModal.AddEditModel), new AddEditItemCommand
                    {
                        Id = _item.Id,
                        ItemArName = _item.ItemArName,
                        ItemCode = _item.ItemCode,
                        ItemNsn = _item.ItemNsn,
                        ItemDescription = _item.ItemDescription,
                        ItemName = _item.ItemName,
                        MeasureUnitId = _item.MeasureUnitId,
                        VoteCodesId = _item.VoteCodesId,
                        VoteCode = _item.VoteCode,
                        ItemClass = _item.ItemClass,
                        SerialNumber = _item.SerialNumber,
                        DateOfEnter = _item.DateOfEnter,
                        EndOfServiceDate = _item.EndOfServiceDate,
                        FirstUseDate = _item.FirstUseDate,
                        MadeIn = _item.MadeIn

                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditItemModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }
        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Are You Sure To Delete This Item?"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await ItemManager.DeleteAsync(id);
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
            _item = new();
            await GetItemsAsync();
        }
        private bool Search(GetAllItemsResponse item)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (item.ItemArName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (item.ItemName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (item.ItemCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (item.ItemNsn?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (item.MeasureName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (item.ItemDescription?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (item.VoteCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            return false;
        }
    }
}
