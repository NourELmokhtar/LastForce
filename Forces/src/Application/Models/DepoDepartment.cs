using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class DepoDepartment : AuditableEntity<int>
    {
        public string Name { get; set; }
        public int ForceID { get; set; }
        [ForeignKey("ForceID")]
        public virtual Forces Force { get; set; }

    }
}
