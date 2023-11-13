
using Blazored.FluentValidation;
using Forces.Application.Features.Bases.Commands.AddEdit;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.BasicInformation.Bases;
using Forces.Client.Infrastructure.Managers.BasicInformation.Forces;
using Forces.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Client.Pages.BasicInformations
{
    public partial class AddEditBaseModal
    {
        [Inject] private IBaseManager BaseManager { get; set; }
        private List<GetAllForcesResponse> _ForceList = new();
        [Parameter] public AddEditBaseCommand AddEditBaseModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Inject] private IForceManager ForceManager { get; set; }



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
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task SaveAsync()
        {
            var response = await BaseManager.SaveAsync(AddEditBaseModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
        }
        protected override async Task OnInitializedAsync()
        {
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
        private async Task LoadDataAsync()
        {
            await GetForcesAsync();
            if (AddEditBaseModel.ForceId == 0)
            {
                AddEditBaseModel.ForceId = _ForceList.FirstOrDefault().Id;
            }
            await Task.CompletedTask;
        }
    }
}
