using Blazored.FluentValidation;
using Forces.Application.Features.PersonalItems.Commands.AddEdit;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.PersonalItems;
using Forces.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Forces.Client.Pages.PersonalItems
{
    public partial class AddEditPersonalItemModal
    {
        [Parameter] public AddEditPersonalItemCommand AddEditItemModel { get; set; } = new();
        [Inject] private IPersonalItemsManager personalItemsManager { get; set; }



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

            var response = await personalItemsManager.SaveAsync(AddEditItemModel);
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
