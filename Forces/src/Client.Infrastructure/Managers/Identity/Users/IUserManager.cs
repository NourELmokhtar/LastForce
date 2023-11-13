using Forces.Application.Enums;
using Forces.Application.Requests.Identity;
using Forces.Application.Responses.Identity;
using Forces.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Identity.Users
{
    public interface IUserManager : IManager
    {
        Task<IResult<List<UserResponse>>> GetAllAsync();
        Task<IResult<List<UserResponse>>> GetAllVoteCodeHoldersAsync(int vCodeId);
        Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request);

        Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);

        Task<IResult<UserResponse>> GetAsync(string userId);

        Task<IResult<UserRolesResponse>> GetRolesAsync(string userId);

        Task<IResult> RegisterUserAsync(RegisterRequest request);
        Task<IResult> EditUserAsync(EditUserRequest request);

        Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request);

        Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request);

        Task<string> ExportToExcelAsync(string searchString = "");
        Task<IResult<UserType>> GetUserType();
        Task<IResult<List<NotificationResponse>>> GetNotifications();
        Task MarkNotificationAsSeen(int notificationId);
        Task MarkNotificationAsRead(int notificationId);
        Task MarkAllNotificationAsSeen();
        Task MarkAllNotificationAsRead();
    }
}