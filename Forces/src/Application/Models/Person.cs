using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class Person : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string NationalNumber { get; set; }
        public string Section { get; set; }
        public string Phone { get; set; }
        public string OfficePhone { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}
