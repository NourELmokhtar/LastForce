using Forces.Application.Responses.VoteCodes;
using Forces.Client.Infrastructure.Managers.VoteCodes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Forces.Client.Pages.Items
{
    public partial class SelectVoteCodesModal
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Inject] private IVoteCodesManager _VoteCodeMnager { get; set; }
        private List<VoteCodeResponse> votecodeList = new();
        private bool _loading = true;
        private string searchString1 = "";
        protected override async Task OnInitializedAsync()
        {
            await GetCodesAsync();
            _loading = false;
        }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        //  private bool FilterFunc1(VoteCodeResponse element) => FilterFunc(element, searchString1);

        private bool FilterFunc(VoteCodeResponse element)
        {
            if (string.IsNullOrWhiteSpace(searchString1))
                return true;
            if (element.UserName.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.VoteCode.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.VoteShortcut.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        private async Task Submit(int ID)
        {
            MudDialog.Close(DialogResult.Ok(ID));
        }
        private async Task GetCodesAsync()
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
    }
}
