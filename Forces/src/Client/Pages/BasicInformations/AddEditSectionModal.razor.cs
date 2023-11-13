using Blazored.FluentValidation;
using Forces.Application.Features.BaseSections.Commands.AddEdit;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Application.Features.Bases.Queries.GetByForceId;
using Forces.Application.Features.Bases.Queries.GetById;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.BasicInformation.Bases;
using Forces.Client.Infrastructure.Managers.BasicInformation.Forces;
using Forces.Client.Infrastructure.Managers.BasicInformation.BaseSections;
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
    public partial class AddEditSectionModal
    {
        [Inject] private IBaseManager BaseManager { get; set; }
        [Inject] private IBaseSectionManager BaseSectionManager { get; set; }
        private List<GetAllForcesResponse> _ForceList = new();
        private List<GetAllBasesByForceIdResponse> _BasesList = new();
        [Parameter] public AddEditBaseSectionCommand AddEditBaseSectionModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Inject] private IForceManager ForceManager { get; set; }
        public int ForceID { get; set; }


        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        private async Task OnForceChanged()
        {
            var response = await BaseManager.GetBasesByForceIdAsync(ForceID);
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
            var response = await BaseSectionManager.SaveAsync(AddEditBaseSectionModel);
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
        Func<int, string> Baseconverter()
        {
            return p => $"{_BasesList.FirstOrDefault(x => x.Id == p).BaseName} | {_BasesList.FirstOrDefault(x => x.Id == p).BaseCode}";
        }
        private async Task LoadDataAsync()
        {
            await GetForcesAsync();
            if (AddEditBaseSectionModel.BaseId == 0)
            {
                ForceID = _ForceList.FirstOrDefault().Id;
                await OnForceChanged();
                AddEditBaseSectionModel.BaseId = _BasesList.FirstOrDefault().Id;
            }
            else
            {
                var Response = await BaseManager.GetBaseByIdAsync(AddEditBaseSectionModel.BaseId);
                if (Response.Succeeded)
                {
                    ForceID = Response.Data.ForceId;
                    await OnForceChanged();
                }
                else
                {
                    foreach (var message in Response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }

            }

            await Task.CompletedTask;
        }
    }
}
