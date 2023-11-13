using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forces.Application.Models;
using Forces.Infrastructure.Models.Identity;
using Forces.Application.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forces.Infrastructure.Models
{
    public class RequestActions : AuditableEntity<int>
    {
        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        public virtual Requests<Models.VoteCodes, Appuser, RequestActions> Request { get; set; }
        public RequestSteps Step { get; set; }
        public string ActionResult { get; set; }
        public string ActionNote { get; set; }
        public bool Seen { get; set; }
        public bool Readed { get; set; }
        public string TargetUserId { get; set; }
        [ForeignKey("TargetUserId")]
        public virtual Appuser TargetUser { get; set; }
        public virtual ICollection<RequestAttachments<VoteCodes, Appuser, RequestActions>> Attachments { get; set; }
        public int? ForceId { get; set; }
        public int? BaseId { get; set; }
        public int? SectionId { get; set; }
        public int? TargetDepartId { get; set; }
        public DepartType? TargetDepartMentType { get; set; }
        public ActionState ActionState { get; set; }
        public ActionsType? ActionsType { get; set; }


    }
}
