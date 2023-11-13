using Forces.Application.Interfaces.Common;
using Forces.Domain.Contracts;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class Building : AuditableEntity<int>
    {
        public Building()
        {
            Rooms = new HashSet<Room>();
        }
        public string BuildingName { get; set; }
        public string BuildingCode { get; set; }
        public int BaseId { get; set; }
        [ForeignKey("BaseId")]
        public virtual Bases Base { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
    