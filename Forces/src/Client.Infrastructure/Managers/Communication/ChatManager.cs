using Forces.Application.Interfaces.Chat;
using Forces.Application.Models.Chat;
using Forces.Application.Responses.Identity;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Communication
{
    public class ChatManager : IChatManager
    {
        private readonly HttpClient _httpClient;

        public ChatManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string cId)
        {
            var response = await _httpClient.GetAsync(Routes.ChatEndpoint.GetChatHistory(cId));
            var data = await response.ToResult<IEnumerable<ChatHistoryResponse>>();
            return data;
        }

        public async Task<IResult<IEnumerable<ChatUserResponse>>> GetChatUsersAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ChatEndpoint.GetAvailableUsers);
            var data = await response.ToResult<IEnumerable<ChatUserResponse>>();
            return data;
        }
        public async Task<IResult<IEnumerable<ChatUserResponse>>> GetChatOUsersAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ChatEndpoint.GetAvailableOUsers);
            var data = await response.ToResult<IEnumerable<ChatUserResponse>>();
            return data;
        }

        public async Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> chatHistory)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ChatEndpoint.SaveMessage, chatHistory);
            var data = await response.ToResult();
            return data;
        }

        public async Task<IResult<List<ChatUserResponse>>> GetAllChatsByUserAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ChatEndpoint.AllChatUserHistory);
            var data = await response.ToResult<List<ChatUserResponse>>();
            return data;
        }

        public async Task<IResult> MarkMessageAsRead(string TargetUserId)
        {
            var response = await _httpClient.GetAsync(Routes.ChatEndpoint.MarkAsRead(TargetUserId));
            var data = await response.ToResult();
            return data;
        }

        public async Task<IResult> MarkAllMessageAsRead()
        {
            var response = await _httpClient.GetAsync(Routes.ChatEndpoint.MarkAllAsRead);
            var data = await response.ToResult();
            return data;
        }

        public async Task<IResult> MarkAllMessageAsSeen()
        {
            var response = await _httpClient.GetAsync(Routes.ChatEndpoint.MarkAllAsSeen);
            var data = await response.ToResult();
            return data;
        }
    }
}