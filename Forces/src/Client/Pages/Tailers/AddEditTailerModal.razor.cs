using Blazored.FluentValidation;
using Forces.Application.Features.Tailers.Commands.AddEdit;
using Forces.Client.Infrastructure.Managers.BasicInformation.Bases;
using Forces.Client.Infrastructure.Managers.Tailers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forces.Application.Features.Bases.Queries.GetAll;
using Forces.Shared.Constants.Application;
using Forces.Client.Extensions;
using System.Security.Claims;

namespace Forces.Client.Pages.Tailers
{
    public partial class AddEditTailerModal
    {

        [Inject] private ITailersManager tailerManager { get; set; }

        [Parameter] public AddEditTailerCommand AddEditTaileModel { get; set; } = new();

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private bool HasBaseId { get; set; } = false;
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        protected override async Task OnInitializedAsync()
        {

            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {


            await Task.CompletedTask;
        }
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {

            var response = await tailerManager.SaveAsync(AddEditTaileModel);
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
    }
}
