using Forces.Application.Interfaces.Chat;
using Forces.Application.Models.Chat;
using System;
using System.Collections.Generic;

namespace Forces.Application.Responses.Identity
{
    public class ChatUserResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string ProfilePictureDataUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool IsOnline { get; set; }
        public string lastMessage { get; set; }
        public bool seen { get; set; } = false;
        public bool Read { get; set; } = false;
        public DateTime? LastMessageDate { get; set; }
        //public virtual ICollection<ChatHistory<IChatUser>> ChatHistoryFromUsers { get; set; }
        //public virtual ICollection<ChatHistory<IChatUser>> ChatHistoryToUsers { get; set; }
    }
}