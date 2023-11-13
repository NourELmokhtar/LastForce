using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class AirCraft : AuditableEntity<int>
    {
        public int AirCraftCode { get; set; }
      
        public string AirCraftName { get; set; }
   
        public int BaseId { get; set; }
        public int SectionId { get; set; }
        public DateTime? DateOfEnter { get; set; }
        public string MadeIN { get; set; }
        public DateTime? LastServes { get; set; }
        public int? Hours { get; set; }
        public int ServesType { get; set; }
        public int AirKindId { get; set; }
        [ForeignKey("AirKindId")]
        public virtual AirKind AirKind { get; set; }
        [ForeignKey("BaseId")]
        public virtual Bases Bases { get; set; }
        [ForeignKey("SectionId")]
        public virtual BasesSections BaseSection { get; set; }
    }
}
