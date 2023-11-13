using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Domain.Contracts
{
    public interface IRequestModel<TUser, TItems, EPriority, TAttachment, TState, TActions, TVoteCode, Tstep> : IRequestModel
        where TUser : IAuditableEntity<string>
        where TItems : IAuditableEntity<int>
        where EPriority : Enum
        where TAttachment : IAuditableEntity<int>, IAttachment
        where TState : Enum
        where TActions : IAuditableEntity<int>
        where TVoteCode : IAuditableEntity<int>
        where Tstep : Enum
    {
        public string RequestRefranceCode { get; set; }
        public TItems Items { get; set; }
        public EPriority Priority { get; set; }
        [ForeignKey("CreatedBy")]
        public TUser user { get; set; }
        public decimal ItemPrice { get; set; }
        public ICollection<TAttachment> Attachments { get; set; }
        public ICollection<TActions> RequestActions { get; set; }
        public TState RequestState { get; set; }
        public string RequestType { get; set; }
        public int VoteCodeId { get; set; }
        [ForeignKey("VoteCodeId")]
        public TVoteCode VoteCode { get; set; }
        public Tstep CurrentStep { get; set; }
        public int? ForceId { get; set; }
        public int? BaseId { get; set; }
        public int? SectionId { get; set; }

    }
    public interface IRequestModel
    {

    }
}
