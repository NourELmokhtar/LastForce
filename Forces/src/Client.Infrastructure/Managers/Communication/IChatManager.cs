using Forces.Application.Interfaces.Chat;
using Forces.Application.Models.Chat;
using Forces.Application.Responses.Identity;
using Forces.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Communication
{
    public interface IChatManager : IManager
    {
        Task<IResult<IEnumerable<ChatUserResponse>>> GetChatUsersAsync();
        Task<IResult<IEnumerable<ChatUserResponse>>> GetChatOUsersAsync();

        Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> chatHistory);

        Task<IResult<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string cId);
        Task<IResult<List<ChatUserResponse>>> GetAllChatsByUserAsync();
        Task<IResult> MarkMessageAsRead(string TargetUserId);
        Task<IResult> MarkAllMessageAsRead();
        Task<IResult> MarkAllMessageAsSeen();
    }
}