using Forces.Domain.Contracts;
using Forces.Infrastructure.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forces.Infrastructure.Models
{

    public class VoteCodeHolders
    {
        public int VoteCodeId { get; set; }
        [ForeignKey("VoteCodeId")]
        public virtual VoteCodes VoteCode { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Appuser User { get; set; }
    }
}
