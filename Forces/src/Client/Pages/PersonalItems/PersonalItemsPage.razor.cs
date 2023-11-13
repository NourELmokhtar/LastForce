using Forces.Application.Features.PersonalItems.Commands.AddEdit;
using Forces.Application.Features.PersonalItems.Queries.DTO;
using Forces.Application.Features.Tailers.Queries;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.PersonalItems;
using Forces.Client.Infrastructure.Managers.Tailers;
using Forces.Shared.Constants.Application;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Forces.Client.Pages.PersonalItems
{
    public partial class PersonalItemsPage
    {

        [Inject] private ITailersManager tailerManager { get; set; }
        [Inject] private IPersonalItemsManager personalItemsManager { get; set; }


        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<TailerDto> _TailerssList = new();
        private TailerDto _Tailer = new();
        private List<PersonalItemDto> _PersonalItemsList = new();
        private PersonalItemDto _Item = new();
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
        public int? CurrentUserBaseId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreate = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PersonalServicesItems.Create)).Succeeded;
            _canEdit = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PersonalServicesItems.Edit)).Succeeded;
            _canDelete = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PersonalServicesItems.Delete)).Succeeded;
            _canSearch = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PersonalServicesItems.Search)).Succeeded;



            await GetTailersAsync();
            await GetDataAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private string TailerNameAndCode(int? id)
        {
            if (id.HasValue)
            {
                var Tailer = _TailerssList.FirstOrDefault(x => x.Id == id);
                return $"{Tailer.Name} | {Tailer.TailerCode}";
            }
            return string.Empty;
        }
        private async Task GetCurrentUserBaseId()
        {
            var currentUserResponse = await _userManager.GetAsync(_currentUser.GetUserId());
            var currentUser = currentUserResponse.Data;
            CurrentUserBaseId = currentUser.BaseId;
        }
        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _Item = _PersonalItemsList.FirstOrDefault(c => c.Id == id);
                if (_Tailer != null)
                {
                    parameters.Add(nameof(AddEditPersonalItemModal.AddEditItemModel), new AddEditPersonalItemCommand
                    {
                        Id = _Item.Id,
                        ItemName = _Item.ItemName,
                        ItemArName = _Item.ItemArName,
                        ItemCode = _Item.ItemCode,
                        BaseId = _Item.BaseId,
                        ItemDescription = _Item.ItemDescription,
                        ItemNsn = _Item.ItemNsn,
                        ItemPrice = _Item.ItemPrice,
                        MaxQtyOnPeriod = _Item.MaxQtyOnPeriod,
                        StorageableItem = _Item.StorageableItem,
                        TailerId = _Item.TailerId,
                        UsagePeriod = _Item.UsagePeriod,
                        UsagePeriodUnit = _Item.UsagePeriodUnit

                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditPersonalItemModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }
        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Are You Sure To Delete This Tailer?"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await tailerManager.DeleteAsync(id);
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
            _Item = new();
            await GetDataAsync();
        }
        private async Task GetTailersAsync()
        {
            var response = await tailerManager.GetAllAsync();
            if (response.Succeeded)
            {
                _TailerssList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task GetDataAsync()
        {
            var response = await personalItemsManager.GetAllAsync();
            if (response.Succeeded)
            {
                _PersonalItemsList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }


        private bool Search(PersonalItemDto item)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (item.ItemName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (item.ItemArName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
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
            return false;
        }
    }
}
