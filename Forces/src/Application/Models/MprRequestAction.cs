using Forces.Application.Enums;
using Forces.Application.Responses.Requets.MPR;
using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class MprRequestAction : AuditableEntity<int>
    {
        public int RequestId { get; set; }
        public MprSteps Step { get; set; }
        public string ActionResult { get; set; }
        public string ActionNote { get; set; }
        public bool Seen { get; set; }
        public bool Readed { get; set; }
        public string TargetUserId { get; set; }
        public virtual ICollection<MprRequestAttachments> Attachments { get; set; }
        public int? ForceId { get; set; }
        public int? BaseId { get; set; }
        public int? SectionId { get; set; }
        public int? TargetDepartId { get; set; }
        public DepartType? TargetDepartMentType { get; set; }
        public ActionState ActionState { get; set; }
        public StepActions? ActionsType { get; set; }
        [ForeignKey("RequestId")]
        public virtual MprRequest Request { get; set; }
        public MprRequestAction()
        {
            Attachments = new HashSet<MprRequestAttachments>();
        }
    }
}
