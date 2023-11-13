using Forces.Application.Enums;
using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class Requests<TVoteCode, TUser, TActionRequest> : AuditableEntity<int>, IRequestModel<TUser, Items, Priority, RequestAttachments<TVoteCode, TUser, TActionRequest>, RequestState, TActionRequest, TVoteCode, RequestSteps>
         where TUser : IAuditableEntity<string>
        where TVoteCode : IAuditableEntity<int>
        where TActionRequest : IAuditableEntity<int>

    {
        public string RequestRefranceCode { get; set; }
        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Items Items { get; set; }
        public Priority Priority { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual TUser user { get; set; }
        public decimal ItemPrice { get; set; }

        public RequestState RequestState { get; set; }
        public string RequestType { get; set; }

        public int VoteCodeId { get; set; }
        [ForeignKey("VoteCodeId")]
        public virtual TVoteCode VoteCode { get; set; }
        public int? ForceId { get; set; }
        public int? BaseId { get; set; }
        public int? SectionId { get; set; }
        public bool isDone { get; set; }
        public string RequestNote { get; set; }
        public RequestSteps CurrentStep { get; set; } = RequestSteps.CreationStep;
        public virtual ICollection<RequestAttachments<TVoteCode, TUser, TActionRequest>> Attachments { get; set; }
        public virtual ICollection<TActionRequest> RequestActions { get; set; }


    }
}
