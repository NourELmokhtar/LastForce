using Blazored.FluentValidation;
using Forces.Application.Features.VehicleRequest.AddEditRequest;
using Forces.Application.Features.VehicleRequest.Dto;
using Forces.Client.Infrastructure.Managers.VehicleRequest;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Forces.Client.Pages.VehicleRequest
{
    public partial class AddRequestPage
    {
        public AddRequestDto Model { get; set; } = new();
        private FluentValidationValidator _fluentValidationValidator;
        [Inject] public IVehicleRequestManager _manager { get; set; }
        private async void Submit() 
        {
            AddEditVehicleRequest request = new AddEditVehicleRequest();
            request.Dto = Model;
            var Response = await _manager.SaveAsync(request);
            if (Response.Succeeded)
            {
                _snackBar.Add(Response.Messages[0],MudBlazor.Severity.Success);
            }
            else
            {
                foreach (var msg in Response.Messages)
                {
                    _snackBar.Add(msg, MudBlazor.Severity.Error);
                }
            }
        }
        protected override Task OnInitializedAsync()
        {
            Model.Packages = new();
            return base.OnInitializedAsync();
        }
    }
}
