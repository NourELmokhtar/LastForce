using Forces.Application.Requests.VoteCodes;
using Forces.Application.Responses.VoteCodes;
using Forces.Client.Extensions;
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


namespace Forces.Client.Pages.VoteCodes
{
    public partial class VoteCodesPage
    {
        [Inject] private IVoteCodesManager VoteCodesManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private List<VoteCodeResponse> _CodesList = new();
        private VoteCodeResponse _code = new();
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
        private bool _canAssign;
        private bool _loaded;
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreate = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VoteCodes.Create)).Succeeded;
            _canEdit = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VoteCodes.Edit)).Succeeded;
            _canDelete = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VoteCodes.Delete)).Succeeded;
            _canSearch = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VoteCodes.Search)).Succeeded;
            _canExport = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VoteCodes.Export)).Succeeded;
            _canAssign = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VoteCodes.Assign)).Succeeded;


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
            var response = await VoteCodesManager.GetAllAsync();
            if (response.Succeeded)
            {
                _CodesList = response.Data.ToList();
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
                _code = _CodesList.FirstOrDefault(c => c.Id == id);
                if (_code != null)
                {
                    parameters.Add(nameof(AddEditVoteCodeModal.AddEditModel), new AddEditVoteCodeRequest
                    {
                        Id = _code.Id,
                        DfaultHolderId = _code.DfaultHolderId,
                        ForceId = _code.ForceId,
                        VoteCode = _code.VoteCode,
                        VoteShortcut = _code.VoteShortcut
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditVoteCodeModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }
        private async Task InvokeUsersModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _code = _CodesList.FirstOrDefault(c => c.Id == id);
                if (_code != null)
                {
                    parameters.Add(nameof(VoteCodeUsers.AddEditModel), new AddEditVoteCodeRequest
                    {
                        Id = _code.Id,
                        DfaultHolderId = _code.DfaultHolderId,
                        ForceId = _code.ForceId,
                        VoteCode = _code.VoteCode,
                        VoteShortcut = _code.VoteShortcut,
                        Holders = _code.Holders.Select(x => x.UserId).ToList()

                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, CloseOnEscapeKey = true, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<VoteCodeUsers>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }
        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Are You Sure To Delete This Vote Code?"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await VoteCodesManager.DeleteAsync(id);
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
            _code = new();
            await GetAllAsync();
        }
        private bool Search(VoteCodeResponse code)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (code.VoteCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (code.VoteShortcut?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (code.UserName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
