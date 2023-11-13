using Forces.Application.Enums;
using Forces.Application.Interfaces.Common;
using Forces.Application.Requests.Identity;
using Forces.Application.Responses.Identity;
using Forces.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Services.Identity
{
    public interface IUserService : IService
    {
        Task<Result<List<UserResponse>>> GetAllAsync();
        Task<Result<List<UserResponse>>> GetAllVoteCodeControllersAsync(int voteCodeId);
        Task<int> GetCountAsync();

        Task<IResult<UserResponse>> GetAsync(string userId);
        Task<IResult<UserResponse>> GetByUsaerNameAsync(string UserName);

        Task<IResult> RegisterAsync(RegisterRequest request, string origin);
        Task<IResult> EditAsync(EditUserRequest request);

        Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request);

        Task<IResult<UserRolesResponse>> GetRolesAsync(string id);

        Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request);

        Task<IResult<string>> ConfirmEmailAsync(string userId, string code);

        Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);

        Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);

        Task<string> ExportToExcelAsync(string searchString = "");
        Task GetVehicleID(string userId);
        Task<IResult<UserType>> GetCurrentUserTypeAsync(string userId);
        Task<Result<List<UserTypeResponse>>> GetAllUserTypesAsync();
        Task<Result<List<NotificationResponse>>> GetNotificationByUserId();
        Task<IResult<DepartType>> GetCurrentUserDepartTypeAsync(string userId);
        Task<int?> GetForceID(string userId);
        Task<int?> GetBaseID(string userId);
        Task<int?> GetBaseSectionID(string userId);
        Task<int?> GetDepartID(string userId);
        Task<int?> GetDefaultVoteCodeId(string userId);

        Task<RequestSteps> GetStepByUserId(string userId);
        Task<RequestSteps> GetStepByUserType(UserType userType);
        Task<RequestSteps> GetCurrentUserStep();
        Task MarkNotificationAsSeen(int notificationId);
        Task MarkNotificationAsRead(int notificationId);
        Task MarkAllNotificationAsSeen();
        Task MarkAllNotificationAsRead();
    }
}