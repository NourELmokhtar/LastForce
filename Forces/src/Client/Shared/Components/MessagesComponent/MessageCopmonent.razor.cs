using AKSoftware.Blazor.Utilities;
using Forces.Application.Responses.Identity;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.Communication;
using Forces.Client.Pages.Communication;
using Forces.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Forces.Client.Shared.Components.MessagesComponent
{
    public partial class MessageCopmonent
    {
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        public List<ChatUserResponse> chatList { get; set; } = new();
        private string CurrentUserId { get; set; }
        [Inject] public IChatManager _ChatManager { get; set; }
        private MudBlazor.MudMenu mnue;
        protected async override Task OnInitializedAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user == null) return;
            CurrentUserId = user.GetUserId();

            await GetMessages();
            HubConnection = new HubConnectionBuilder()
                .WithUrl(_navigationManager.ToAbsoluteUri(ApplicationConstants.SignalR.HubUrl))
                .Build();
            HubConnection.On<string, string, string>(ApplicationConstants.SignalR.ReceiveChatNotification, (message, receiverUserId, senderUserId) =>
            {
                if (CurrentUserId == receiverUserId)
                {
                    if (!_navigationManager.Uri.Contains($"chat/{senderUserId}"))
                    {
                        GetMessages().Wait();
                        StateHasChanged();
                    }
                }
            });
        }
        private async Task markAllAsSeen()
        {
            await _ChatManager.MarkAllMessageAsSeen();
            await GetMessages();
            StateHasChanged();
        }
        public void GetNewMesages()
        {
            MessagingCenter.Subscribe<Chat, string>(this, "NewMessages", async (sender, value) =>
            {
                // Do actions against the value 
                // If the value is updating the component make sure to call 
                await GetMessages();
                StateHasChanged(); // To update the state of the component 
            });
            MessagingCenter.Subscribe<MainLayout, string>(this, "NewMessages", async (sender, value) =>
            {
                // Do actions against the value 
                // If the value is updating the component make sure to call 
                await GetMessages();
                StateHasChanged(); // To update the state of the component 
            });
        }
        public void SubscribeToMarkAsRead()
        {
            MessagingCenter.Subscribe<Chat, string>(this, "MarkAsRead", async (sender, value) =>
             {
                 // Do actions against the value 
                 // If the value is updating the component make sure to call 
                 await markAsRead(value);
                 await markAllAsSeen();
                 StateHasChanged(); // To update the state of the component 
             });
        }
        public void SubscribeToMarkAsSeen()
        {
            MessagingCenter.Subscribe<Chat, string>(this, "MarkAsSeen", async (sender, value) =>
            {
                // Do actions against the value 
                // If the value is updating the component make sure to call 
                await markAllAsSeen();
                StateHasChanged(); // To update the state of the component 
            });
        }
        private async Task markAsRead(string userId)
        {
            await _ChatManager.MarkMessageAsRead(userId);
            await GetMessages();
            StateHasChanged();
        }
        private async Task GoToMessage(string userID)
        {
            mnue.CloseMenu();
            await markAsRead(userID);
            _navigationManager.NavigateTo($"chat/{userID}");
        }
        private async Task GetMessages()
        {
            var response = await _ChatManager.GetAllChatsByUserAsync();
            if (response.Succeeded)
            {
                chatList = response.Data;
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
            count = chatList.Where(x => x.seen == false).Count();
            return count;
        }
    }
}
