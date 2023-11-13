using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forces.Application.Requests.VoteCodes;
using Forces.Client.Infrastructure.Managers.VoteCodes;
using MudBlazor;
using Microsoft.AspNetCore.SignalR.Client;
using Blazored.FluentValidation;
using Forces.Shared.Constants.Application;
using Forces.Client.Extensions;

namespace Forces.Client.Pages.VoteCodes
{
    public partial class AddTransactionModal
    {
        [Parameter] public AddEditVcodeTransactionRequest AddModel { get; set; }
        [Inject] public IVoteCodesManager _voteCodeManager { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        public void Cancel()
        {
            MudDialog.Cancel();
        }
        protected override async Task OnInitializedAsync()
        {

            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private async Task SaveAsync()
        {

            var response = await _voteCodeManager.AddEditTransaction(AddModel);
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
