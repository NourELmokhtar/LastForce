using Blazored.FluentValidation;
using Forces.Application.Features.Color.Commands.AddEdit;
using Forces.Client.Infrastructure.Managers.Color;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace Forces.Client.Pages.Colors
{
    public partial class AddEditColorsModal
    {
        [Inject] public IColorManager _colorManager { get; set; }
        [Parameter] public AddEditColorCommand Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {

            var response = await _colorManager.SaveAsync(Model);
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
