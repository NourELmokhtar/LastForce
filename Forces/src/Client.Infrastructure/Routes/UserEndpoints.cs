namespace Forces.Client.Infrastructure.Routes
{
    public static class UserEndpoints
    {
        public static string GetAll = "api/identity/user";
        public static string GetAllHolders(int vCodeId) => $"api/identity/user/Holders/{vCodeId}";

        public static string Get(string userId)
        {
            return $"api/identity/user/{userId}";
        }

        public static string GetUserRoles(string userId)
        {
            return $"api/identity/user/roles/{userId}";
        }

        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/identity/user/export";
        public static string Register = "api/identity/user";
        public static string Edit = "api/identity/user/edit-user";
        public static string ToggleUserStatus = "api/identity/user/toggle-status";
        public static string ForgotPassword = "api/identity/user/forgot-password";
        public static string ResetPassword = "api/identity/user/reset-password";
        public static string GetUserType = "api/identity/user/GetUserType";
        public static string GetUserNotifications = "api/identity/user/Notifications";
        public static string MarkNotificationAsSeen(int Id) => $"api/identity/user/MarkNotificationAsSeen/{Id}";
        public static string MarkNotificationAsRead(int Id) => $"api/identity/user/MarkNotificationAsRead/{Id}";
        public static string MarkAllNotificationAsSeen = "api/identity/user/MarkAllNotificationAsSeen";
        public static string MarkAllNotificationAsRead = "api/identity/user/MarkAllNotificationAsRead";

    }
}