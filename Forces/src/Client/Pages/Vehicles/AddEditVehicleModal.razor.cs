using Forces.Client.Infrastructure.Managers.Vehicles;
using Microsoft.AspNetCore.Components;
using Forces.Application.Features.Vehicle.Commands.AddEdit;
using MudBlazor;
using Blazored.FluentValidation;
using Forces.Client.Pages.Tailers;
using Forces.Shared.Constants.Application;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using Forces.Client.Infrastructure.Managers.Color;
using Forces.Application.Features.Color.Queries.GetAll;
using System.Collections.Generic;
using System.Linq;
using System;
using Forces.Client.Pages.Colors;

namespace Forces.Client.Pages.Vehicles
{
    public partial class AddEditVehicleModal
    {
        [Inject] public IVehicleManager _vehicleManager { get; set; }
        [Inject] public IColorManager _colorManager { get; set; }
        [Parameter] public AddEditVehicleCommand Model { get; set; } = new();
        private List<GetAllColorResponse> ColorList = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        protected override async Task OnInitializedAsync()
        {
            await GetAllColors();
        }
        private async Task GetAllColors() 
        {
            var Response = await _colorManager.GetAllAsync();
            if (Response.Succeeded)
            {
                ColorList = Response.Data;
                if (Model.Id == 0)
                {
                    Model.ColorID = ColorList.FirstOrDefault().Id;
                    //StateHasChanged();
                }
            }
            else
            {
                foreach (var msg in Response.Messages)
                {
                    _snackBar.Add(msg, Severity.Error);
                }
            }
        }
        Func<int, string> converter =>
        
        p =>  ColorList.FirstOrDefault(x => x.Id == p).ColorName;

        private async Task InvokeColorModal() 
        {
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = false, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditColorsModal>( _localizer["Create"], options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await GetAllColors();
                Model.ColorID = ColorList.Max(x => x.Id);
            }
        }
        private async Task SaveAsync()
        {

            var response = await _vehicleManager.SaveAsync(Model);
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
            
        }
    }
}
