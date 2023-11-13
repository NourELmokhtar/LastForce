using Blazored.FluentValidation;
using FluentValidation;
using Forces.Application.Features.BaseSections.Queries.GetAll;
using Forces.Application.Features.House.Queries.GetAll;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Application.Features.House.Queries.GetAll;
using Forces.Application.Features.House.Commands.AddEdit;
using Forces.Application.Features.Room.Queries.GetAll;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.BasicInformation.BaseSections;
using Forces.Client.Infrastructure.Managers.BasicInformation.Forces;
using Forces.Client.Infrastructure.Managers.House;
using Forces.Client.Infrastructure.Managers.House;
using Forces.Client.Infrastructure.Managers.House;
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

namespace Forces.Client.Pages.House
{
    public partial class AddEditHouseModal
    {
        public AddEditHouseModal()
        {

        }
        [Inject] private IHouseManager HouseManager { get; set; }
        [Inject] private IBaseManager BaseManager { get; set; }
        private List<GetAllBasesResponse> _BaseList = new();

        private List<GetAllForcesResponse> _ForceList = new();
        [Parameter] public AddEditHouseCommand AddEditHouseModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Inject] private IForceManager ForceManager { get; set; }
        private string selectedBase;

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
        private async Task GetBasesAsync()
        {
            var response = await BaseManager.GetAllAsync();
            if (response.Succeeded)
            {
                _BaseList = response.Data.ToList();
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
            AddEditHouseModel.BaseId = (int)converterForBases(selectedBase);
            var response = await HouseManager.SaveAsync(AddEditHouseModel);
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
        private int? converterForBases(string ss)
        {
            return _BaseList.FirstOrDefault(s => s.BaseName == ss).Id;
        }


        private async Task LoadDataAsync()
        {
            await GetBasesAsync();
            await GetForcesAsync();

            await Task.CompletedTask;
        }
    }

}
