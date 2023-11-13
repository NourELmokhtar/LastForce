using Forces.Application.Features.PersonalItemOperations.Queries.Dto;
using Forces.Application.Features.PersonalItemOperations.Queries.GetByFillter;
using Forces.Application.Responses.Identity;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.PersonalItemOperations;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Forces.Client.Pages.PersonalItemsOperations.Components
{
    public partial class AllOperations
    {
        [Inject] public IPersonalItemOperationManager operationsManager { get; set; }
        public List<PersonalItemOperationDto> OperationsList { get; set; } = new();
        public PersonalItemOperationDto Operation { get; set; } = new();
        public List<UserResponse> UsersList { get; set; } = new();
        public UserResponse _currentAppUser { get; set; } = new();
        public GetPersonalItemsOperationsByFillter Fillter { get; set; } = new();
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
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreate = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PersonalServicesItems.Create)).Succeeded;
            _canEdit = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PersonalServicesItems.Edit)).Succeeded;
            _canDelete = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PersonalServicesItems.Delete)).Succeeded;
            _canSearch = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PersonalServicesItems.Search)).Succeeded;
            Fillter = new GetPersonalItemsOperationsByFillter();
            await GetUsersDataAsync();
            await GetCurrentUserBaseId();
            await GetDataAsync();

            _loaded = true;
        }
        private string GetUserName(string UserId)
        {
            if (!string.IsNullOrEmpty(UserId))
            {
                var user = UsersList.FirstOrDefault(x => x.Id == UserId);
                return $"{user.UserName}";
            }
            return string.Empty;
        }

        private async Task InvokeModal()
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(FillterModal.Fillter), new GetPersonalItemsOperationsByFillter
            {
                BaseId = _currentAppUser.BaseId,
                ForceId = _currentAppUser.ForceId,
                BaseSectionId = _currentAppUser.BaseSectionId
            });
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<FillterModal>(_localizer["Fillter Operations"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var FillterReult = result.Data as GetPersonalItemsOperationsByFillter;
                await GetDataAsync(FillterReult);
            }
        }
        private async Task GetCurrentUserBaseId()
        {
            var currentUserResponse = await _userManager.GetAsync(_currentUser.GetUserId());
            var currentUser = currentUserResponse.Data;
            _currentAppUser = currentUser;
        }
        private async Task GetDataAsync(GetPersonalItemsOperationsByFillter FilterModel = null)
        {
            if (FilterModel == null)
            {
                Fillter.BaseId = _currentAppUser.BaseId;
                Fillter.ForceId = _currentAppUser.ForceId;
                Fillter.BaseSectionId = _currentAppUser.BaseSectionId;

            }
            else
            {
                Fillter = FilterModel;
            }
            var response = await operationsManager.GetAllFillterAsync(Fillter);
            if (response.Succeeded)
            {
                OperationsList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task LoadDataAsync()
        {
            await GetDataAsync();
        }
        private async Task GetUsersDataAsync()
        {
            var response = await _userManager.GetAllAsync();
            if (response.Succeeded)
            {
                UsersList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

    }
}
