using Forces.Application.Responses.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Forces.Client.Pages.PersonalItemsOperations
{
    public partial class AssignItemsPage
    {
        [Parameter] public string UserId { get; set; }
        public UserResponse user { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            await GetUserInfo();
        }

        private async Task GetUserInfo()
        {
            var Response = await _userManager.GetAsync(UserId);
            if (Response.Succeeded)
            {
                user = Response.Data;
            }
            else
            {
                foreach (var msg in Response.Messages)
                {
                    _snackBar.Add(msg, MudBlazor.Severity.Error);
                }

            }

        }
    }
}
