using Forces.Application.Enums;
using Forces.Application.Requests.Requests;
using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class MprRequest : AuditableEntity<int>
    {
        public string RequestRefranceCode { get; set; }
        public Priority Priority { get; set; }
        public RequestState RequestState { get; set; }
        public int VoteCodeId { get; set; }
        public int? ForceId { get; set; }
        public int? BaseId { get; set; }
        public int? SectionId { get; set; }
        public bool isDone { get; set; }
        public bool? Paied { get; set; }
        public bool? ConfirmPaied { get; set; }
        public string RequestNote { get; set; }
        public MprSteps CurrentStep { get; set; } = MprSteps.CreationStep;
        public virtual ICollection<MprRequestAttachments> Attachments { get; set; }
        public virtual ICollection<MprRequestAction> RequestActions { get; set; }
        public virtual ICollection<RequestItem> RequestItems { get; set; }
        public MprRequest()
        {
            Attachments = new HashSet<MprRequestAttachments>();
            RequestActions = new HashSet<MprRequestAction>();
            RequestItems = new HashSet<RequestItem>();
        }
    }
}
