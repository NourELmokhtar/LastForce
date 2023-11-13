using Forces.Domain.Contracts;
using Forces.Infrastructure.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Models
{
    public class NotificationsUsers : AuditableEntity<int>
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public bool Seen { get; set; }
        public bool Readed { get; set; }
        public string NotificationType { get; set; }
        public string EntityPrimaryKey { get; set; }
        public string RefUrl { get; set; }
        public string CustomUrl { get; set; }
        public string TargetUserId { get; set; }
        [ForeignKey("TargetUserId")]
        public virtual Appuser TargetUser { get; set; }
    }
}
