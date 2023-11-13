using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class RequestAttachments<TVoteCode, TUser, TActions> : AuditableEntity<int>, IAttachment<Models.Requests<TVoteCode, TUser, TActions>>
        where TUser : IAuditableEntity<string>
        where TVoteCode : IAuditableEntity<int>
        where TActions : IAuditableEntity<int>

    {
        public string FileUrl { get; set; }
        public int? RequestID { get; set; }
        public int? ActionId { get; set; }
        public string AttachmentType { get; set; }
        [ForeignKey("RequestID")]
        public virtual Requests<TVoteCode, TUser, TActions> Request { get; set; }
        [ForeignKey("ActionId")]
        public virtual TActions Action { get; set; }
        public bool? Selected { get; set; }

    }
    public class MprRequestAttachments : AuditableEntity<int>
    {
        public string FileUrl { get; set; }
        public int? RequestID { get; set; }
        public int? ActionId { get; set; }
        public string AttachmentType { get; set; }
        public bool? Selected { get; set; }
        [ForeignKey("RequestID")]
        public virtual MprRequest Request { get; set; }
        [ForeignKey("ActionId")]
        public virtual MprRequestAction Action { get; set; }

    }
}
