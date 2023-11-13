using Forces.Application.Models;
using Forces.Domain.Contracts;
using Forces.Infrastructure.Models.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forces.Infrastructure.Models
{

    public class VoteCodes : AuditableEntity<int>
    {
        public VoteCodes()
        {
            VoteCodeHolders = new HashSet<Appuser>();
            Items = new HashSet<Items>();
            Logs = new HashSet<VoteCodeLog>();
            Requests = new HashSet<Requests<VoteCodes, Appuser, RequestActions>>();
        }
        public string VoteCode { get; set; }
        public string VoteShortcut { get; set; }
        public string DefaultHolderUserId { get; set; }
        [ForeignKey("DefaultHolderUserId")]
        public virtual Appuser DefaultHolder { get; set; }
        public int ForceID { get; set; }
        public virtual Application.Models.Forces Force { get; set; }
        public decimal CreditAmount { get; set; }
        public virtual ICollection<Appuser> VoteCodeHolders { get; set; }
        public virtual ICollection<Items> Items { get; set; }
        public virtual ICollection<VoteCodeLog> Logs { get; set; }
        public virtual ICollection<Requests<VoteCodes, Appuser, RequestActions>> Requests { get; set; }
    }

}
