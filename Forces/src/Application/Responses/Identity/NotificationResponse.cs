using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Responses.Identity
{
    public class NotificationResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Seen { get; set; }
        public bool Readed { get; set; }
        public string NotificationType { get; set; }
        public string EntityPrimaryKey { get; set; }
        public string RefUrl { get; set; }
        public string CustomUrl { get; set; }
        public string TargetUserId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
