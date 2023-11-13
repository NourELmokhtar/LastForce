using Forces.Application.Responses.Identity;
using Forces.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Forces.Client.Shared.Components
{
    public partial class NotificationComponent
    {
        public List<NotificationResponse> NotificationList { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private MudBlazor.MudMenu mnue;
        protected async override Task OnInitializedAsync()
        {
            await GetNotifications();
            HubConnection = new HubConnectionBuilder()
                            .WithUrl(_navigationManager.ToAbsoluteUri(ApplicationConstants.SignalR.HubUrl))
                            .Build();
            HubConnection.On(ApplicationConstants.SignalR.NotifyOwner, async () =>
            {
                await GetNotifications();

                StateHasChanged();
            });
            HubConnection.On(ApplicationConstants.SignalR.NotifySteppers, async () =>
            {
                await GetNotifications();

                StateHasChanged();
            });
            await HubConnection.StartAsync();

        }
        private async Task GetNotifications()
        {
            var response = await _userManager.GetNotifications();
            if (response.Succeeded)
            {
                NotificationList = response.Data;
            }
            else
            {
                foreach (var Message in response.Messages)
                {
                    _snackBar.Add(Message, MudBlazor.Severity.Error);
                }
            }
            UnSeenCounter();
        }
        private int UnSeenCounter()
        {
            int count = 0;
            count = NotificationList.Where(x => x.Seen == false).Count();
            return count;
        }
        private async Task markAsSeen(int Id)
        {
            await _userManager.MarkNotificationAsSeen(Id);
            await GetNotifications();
            StateHasChanged();
        }
        private async Task markAsRead(int Id)
        {
            await _userManager.MarkNotificationAsRead(Id);
            await GetNotifications();
            StateHasChanged();
        }
        private async Task markAllAsSeen()
        {

            await _userManager.MarkAllNotificationAsSeen();
            await GetNotifications();
            StateHasChanged();

        }
        private async Task markAllAsRead()
        {
            await _userManager.MarkAllNotificationAsRead();
            await GetNotifications();
            StateHasChanged();

        }
        private async Task GoToNotification(int id, string Url)
        {
            mnue.CloseMenu();
            await markAsRead(id);
            _navigationManager.NavigateTo(Url);
        }

    }
}
