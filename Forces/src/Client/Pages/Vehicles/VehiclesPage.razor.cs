using Forces.Application.Features.Tailers.Commands.AddEdit;
using Forces.Application.Features.Tailers.Queries;
using Forces.Application.Features.Vehicle.Queries.GetAll;
using Forces.Client.Infrastructure.Managers.Vehicles;
using Forces.Client.Pages.Tailers;
using Forces.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Forces.Application.Features.Vehicle.Commands.AddEdit;

namespace Forces.Client.Pages.Vehicles
{

    public partial class VehiclesPage
    {
        [Inject] public IVehicleManager _vehicleManager { get; set; }
        [Inject] public IMapper _mapper { get; set; }
        
        private List<GetAllVehicleResponse> _vehiclesList = new();
        private GetAllVehicleResponse _vehicle = new();
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = true;
        private bool _canCreate = true;
        private bool _canEdit = true;
        private bool _canDelete = true;
        private bool _canSearch = true;
        private bool _loaded;
        protected async override Task OnInitializedAsync()
        {
           await GetAllVehicles();
            _loaded = true;
        }
        private async Task GetAllVehicles()
        {
            var Response = await _vehicleManager.GetAllAsync();
            if (Response.Succeeded)
            {
                _vehiclesList = Response.Data;
            }
            else
            {
                foreach (var item in Response.Messages)
                {
                    _snackBar.Add(item, MudBlazor.Severity.Error);
                }
            }
        }
        private async Task Reset() 
        {
            _vehicle = new();
            await GetAllVehicles();
        }
        private async Task InvokeModal(int id =0) 
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _vehicle = _vehiclesList.FirstOrDefault(c => c.Id == id);
                if (_vehicle != null)
                {
                    parameters.Add(nameof(AddEditVehicleModal.Model), _mapper.Map<AddEditVehicleCommand>(_vehicle));
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditVehicleModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }
        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Are You Sure To Delete This Vehicle?"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await _vehicleManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                   
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

        private bool Search(GetAllVehicleResponse vehicle)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (vehicle.AdditionalNumber?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (vehicle.BattryType.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (vehicle.EngineNo?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (vehicle.VIN?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (vehicle.ColorName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (vehicle.FuleType.ToString()?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (vehicle.MadeIn?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (vehicle.WheelsSize?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (vehicle.VehicleName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (vehicle.VehicleNumber?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (vehicle.Year?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (vehicle.WheelsYear?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
