using Forces.Application.Interfaces.Chat;
using Forces.Application.Models.Chat;
using Forces.Application.Responses.Identity;
using Forces.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<Result<IEnumerable<ChatUserResponse>>> GetChatUsersAsync(string userId);
        Task<Result<IEnumerable<ChatUserResponse>>> GetChatOUsersAsync(string userId);
        Task<Result<List<ChatUserResponse>>> GetAllChatsByUserAsync(string userId);
        Task<IResult> MarkMessageAsRead(string userId, string TargetUserId);
        Task<IResult> MarkAllMessageAsRead(string userId);
        Task<IResult> MarkAllMessageAsSeen(string userId);

        Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> message);

        Task<Result<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string userId, string contactId);
    }
}