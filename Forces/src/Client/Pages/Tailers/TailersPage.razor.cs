using Forces.Application.Features.Bases.Queries.GetAll;
using Forces.Application.Features.Tailers.Commands.AddEdit;
using Forces.Application.Features.Tailers.Queries;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.BasicInformation.Bases;
using Forces.Client.Infrastructure.Managers.Identity.Users;
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


namespace Forces.Client.Pages.Tailers
{
    public partial class TailersPage
    {
        [Inject] private IBaseManager baseManager { get; set; }
        [Inject] private ITailersManager tailerManager { get; set; }
        [Inject] private IUserManager userManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private List<GetAllBasesResponse> _BasesList = new();
        private GetAllBasesResponse _base = new();
        private List<TailerDto> _TailerssList = new();
        private TailerDto _Tailer = new();
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;
        private ClaimsPrincipal _currentUser;
        private bool _canCreateTailer;
        private bool _canEditTailer;
        private bool _canDeleteTailer;
        private bool _canSearchTailer;
        private bool _loaded;
        public int? CurrentUserBaseId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateTailer = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Tailer.Create)).Succeeded;
            _canEditTailer = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Tailer.Edit)).Succeeded;
            _canDeleteTailer = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Tailer.Delete)).Succeeded;
            _canSearchTailer = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Tailer.Search)).Succeeded;

            //await GetCurrentUserBaseId();
            await GetBasesAsync();
            await GetTailersAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private string BaseNameAndCode(int id)
        {
            var Base = _BasesList.FirstOrDefault(x => x.Id == id);
            return $"{Base.BaseName} | {Base.BaseCode}";
        }
        private async Task GetCurrentUserBaseId()
        {
            var currentUserResponse = await userManager.GetAsync(_currentUser.GetUserId());
            var currentUser = currentUserResponse.Data;
            CurrentUserBaseId = currentUser.BaseId;
        }
        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _Tailer = _TailerssList.FirstOrDefault(c => c.Id == id);
                if (_Tailer != null)
                {
                    parameters.Add(nameof(AddEditTailerModal.AddEditTaileModel), new AddEditTailerCommand
                    {
                        Id = _Tailer.Id,
                        Name = _Tailer.Name,
                        Phone = _Tailer.Phone,
                        BaseId = _Tailer.BaseId,
                        TailerCode = _Tailer.TailerCode
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditTailerModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
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
            _Tailer = new TailerDto();
            await GetTailersAsync();
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
        private bool Search(TailerDto tailer)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (tailer.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (tailer.TailerCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (tailer.Phone?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (_BasesList.Where(x => x.BaseName.Contains(_searchString, StringComparison.OrdinalIgnoreCase)).Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
