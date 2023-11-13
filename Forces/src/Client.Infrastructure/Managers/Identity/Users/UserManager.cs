using Forces.Application.Enums;
using Forces.Application.Requests.Identity;
using Forces.Application.Responses.Identity;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Identity.Users
{
    public class UserManager : IUserManager
    {
        private readonly HttpClient _httpClient;

        public UserManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<UserResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.UserEndpoints.GetAll);
            return await response.ToResult<List<UserResponse>>();
        }

        public async Task<IResult<UserResponse>> GetAsync(string userId)
        {
            var response = await _httpClient.GetAsync(Routes.UserEndpoints.Get(userId));
            return await response.ToResult<UserResponse>();
        }

        public async Task<IResult> RegisterUserAsync(RegisterRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoints.Register, request);
            return await response.ToResult();
        }

        public async Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoints.ToggleUserStatus, request);
            return await response.ToResult();
        }

        public async Task<IResult<UserRolesResponse>> GetRolesAsync(string userId)
        {
            var response = await _httpClient.GetAsync(Routes.UserEndpoints.GetUserRoles(userId));
            return await response.ToResult<UserRolesResponse>();
        }

        public async Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.UserEndpoints.GetUserRoles(request.UserId), request);
            return await response.ToResult<UserRolesResponse>();
        }

        public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoints.ForgotPassword, model);
            return await response.ToResult();
        }

        public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoints.ResetPassword, request);
            return await response.ToResult();
        }

        public async Task<string> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.UserEndpoints.Export
                : Routes.UserEndpoints.ExportFiltered(searchString));
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<IResult<UserType>> GetUserType()
        {
            var response = await _httpClient.GetAsync(Routes.UserEndpoints.GetUserType);
            return await response.ToResult<UserType>();
        }

        public async Task<IResult<List<UserResponse>>> GetAllVoteCodeHoldersAsync(int vCodeId)
        {
            var response = await _httpClient.GetAsync(Routes.UserEndpoints.GetAllHolders(vCodeId));
            return await response.ToResult<List<UserResponse>>();
        }

        public async Task<IResult<List<NotificationResponse>>> GetNotifications()
        {
            var response = await _httpClient.GetAsync(Routes.UserEndpoints.GetUserNotifications);
            return await response.ToResult<List<NotificationResponse>>();
        }

        public async Task MarkNotificationAsSeen(int notificationId)
        {
            await _httpClient.GetAsync(Routes.UserEndpoints.MarkNotificationAsSeen(notificationId));
        }

        public async Task MarkNotificationAsRead(int notificationId)
        {
            await _httpClient.GetAsync(Routes.UserEndpoints.MarkNotificationAsRead(notificationId));
        }

        public async Task MarkAllNotificationAsSeen()
        {
            await _httpClient.GetAsync(Routes.UserEndpoints.MarkAllNotificationAsSeen);
        }

        public async Task MarkAllNotificationAsRead()
        {
            await _httpClient.GetAsync(Routes.UserEndpoints.MarkAllNotificationAsRead);
        }

        public async Task<IResult> EditUserAsync(EditUserRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoints.Edit, request);
            return await response.ToResult();
        }
    }
}