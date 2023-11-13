
using Blazored.FluentValidation;
using Forces.Application.Features.Items.Commands.AddEdit;
using Forces.Application.Features.MeasureUnits.Queries.GetAll;
using Forces.Application.Responses.VoteCodes;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.Items;
using Forces.Client.Infrastructure.Managers.Items.MeasureUnits;
using Forces.Client.Infrastructure.Managers.VoteCodes;
using Forces.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Forces.Client.Pages.Items
{
    public partial class AddEditItemModal
    {
        [Inject] private IItemsManager ItemManager { get; set; }
        [Inject] private IVoteCodesManager _VoteCodeMnager { get; set; }
        private List<GetAllMeasureUnitsResponse> _UnitsList = new();
        [Parameter] public AddEditItemCommand AddEditModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Inject] private IMeasureUnitsManager UnitsManager { get; set; }
        private List<VoteCodeResponse> votecodeList = new();

        [Inject] IDialogService DialogService { get; set; }
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        private string VoteCodestr(int Id)
        {
            var code = votecodeList.FirstOrDefault(x => x.Id == Id);
            if (code == null)
            {
                return "";
            }
            return code.VoteCode;
        }
        private async Task GetUnitsAsync()
        {
            var response = await UnitsManager.GetAllAsync();
            if (response.Succeeded)
            {
                _UnitsList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task GetVoteCodesAsync()
        {
            var response = await _VoteCodeMnager.GetAllAsync();
            if (response.Succeeded)
            {
                votecodeList = response.Data.ToList();
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
            var response = await ItemManager.SaveAsync(AddEditModel);
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
            return p => $"{_UnitsList.FirstOrDefault(x => x.Id == p).Name}";
        }
        Func<int, string> VoteCodeconverter()
        {
            return p => $"{votecodeList.FirstOrDefault(x => x.Id == p).VoteCode}";
        }
        private async Task SelectVoteCode()
        {
            var dialog = DialogService.Show<SelectVoteCodesModal>();
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                AddEditModel.VoteCodesId = int.Parse(result.Data.ToString());
                AddEditModel.VoteCode = votecodeList.FirstOrDefault(x => x.Id == AddEditModel.VoteCodesId).VoteCode;
            }
        }

        private async Task LoadDataAsync()
        {
            await GetUnitsAsync();
            await GetVoteCodesAsync();
            if (AddEditModel.MeasureUnitId == 0 && _UnitsList.Count > 0)
            {
                AddEditModel.MeasureUnitId = _UnitsList.FirstOrDefault().Id;
            }
            await Task.CompletedTask;
        }
    }
}
