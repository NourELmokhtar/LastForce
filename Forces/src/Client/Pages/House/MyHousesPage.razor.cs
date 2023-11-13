using Forces.Application.Features.Bases.Commands.AddEdit;
using Forces.Application.Features.Bases.Queries.GetAll;
using Forces.Application.Features.BaseSections.Queries.GetAll;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Application.Features.House.Commands.AddEdit;
using Forces.Application.Features.House.Queries.GetAll;
using Forces.Application.Interfaces.Repositories;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.BasicInformation.Bases;
using Forces.Client.Infrastructure.Managers.BasicInformation.BaseSections;
using Forces.Client.Infrastructure.Managers.BasicInformation.Forces;
using Forces.Client.Infrastructure.Managers.House;
using Forces.Client.Pages.BasicInformations;
using Forces.Shared.Constants.Application;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Security.Claims;

namespace Forces.Client.Pages.House
{
    public partial class MyHousesPage
    {
        [Inject] private IHouseManager HouseManager { get; set; }
        [Inject] private IForceManager ForceManager { get; set; }
        [Inject] private IBaseSectionManager BaseSectionManager { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private readonly IUnitOfWork<int> _unitOfWork;
        private List<GetAllHousesResponse> _HousesList = new();
        private List<GetAllBasesSectionsQueryResponse> _BaseSectionList = new();
        private GetAllHousesResponse _House = new();
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
            _canCreateBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.House.Create)).Succeeded;
            _canEditBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.House.Edit)).Succeeded;
            _canDeleteBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.House.Delete)).Succeeded;
            _canSearchBase = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.House.Search)).Succeeded;
            await GetBasesAsync();
            await GetForcesAsync();
            await GetHousesAsync();

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
        private async Task GetHousesAsync()
        {
            var response = await HouseManager.GetAllAsync();
            if (response.Succeeded)
            {
                _HousesList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private TableGroupDefinition<GetAllHousesResponse> _groupDefinition = new()
        {
            GroupName = "Force",
            Indentation = true,
            Expandable = true,
            Selector = (e) => e.HouseName
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
                _House = _HousesList.FirstOrDefault(c => c.Id == id);
                if (_House != null)
                {
                    parameters.Add(nameof(AddEditHouseModal.AddEditHouseModel), new AddEditHouseCommand
                    {
                        Id = _House.Id,
                        HouseName = _House.HouseName,
                        HouseCode = _House.HouseCode,
                        BaseId = _unitOfWork.Repository<Application.Models.Bases>().GetAllAsync().Result.Where(y => y.BaseName == _House.BaseName).FirstOrDefault().Id,
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditHouseModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
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
                var response = await HouseManager.DeleteAsync(id);
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
            _House = new GetAllHousesResponse();
            await GetHousesAsync();
        }
        private bool Search(GetAllHousesResponse Base)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (Base.HouseName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            if (Base.HouseName.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}