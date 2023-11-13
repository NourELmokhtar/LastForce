namespace Forces.Client.Infrastructure.Routes
{
    public static class ChatEndpoint
    {
        public static string GetAvailableUsers = "api/chats/users";
        public static string GetAvailableOUsers = "api/chats/Ousers";
        public static string SaveMessage = "api/chats";
        public static string MarkAllAsRead = "api/chats/markallasread";
        public static string MarkAllAsSeen = "api/chats/markAllAsSeen";
        public static string AllChatUserHistory = "api/chats/chatusers";

        public static string GetChatHistory(string userId)
        {
            return $"api/chats/{userId}";
        }
        public static string MarkAsRead(string TargetuserId)
        {
            return $"api/chats/MarkAsRead/{TargetuserId}";
        }
    }
}