

using Forces.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Forces.Application.Models
{
    public class BinRack : AuditableEntity<int>
    {
        public int BinCode { get; set; }
        public string BinName { get; set; }
        [ForeignKey("ShilfId")]
        public virtual ShilfRack Store { get; set; }
        public int ShilfId { get; set; }
        public string Name { get; internal set; }
    }
}
