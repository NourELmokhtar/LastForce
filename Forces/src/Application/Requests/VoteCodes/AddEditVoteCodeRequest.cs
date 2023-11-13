using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Requests.VoteCodes
{
    public class AddEditVoteCodeRequest
    {
        public int Id { get; set; }
        [Required]
        public string VoteCode { get; set; }
        [Required]
        public string VoteShortcut { get; set; }
        [Required]
        public int ForceId { get; set; }
        public List<string> Holders { get; set; } = new();
        public string DfaultHolderId { get; set; }
        public decimal CreditAmount { get; set; } = decimal.Zero;
    }
}
