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
    public class Bases : AuditableEntity<int>
    {
        public string BaseName { get; set; }
        public string BaseCode { get; set; }
        public int ForceId { get; set; }
        [ForeignKey("ForceId")]
        public virtual Forces Force { get; set; }
        public virtual ICollection<BasesSections> Sections { get; set; }
        public UserType VehicleRequestUserType { get; set; } = UserType.OCLogAdmin;

    }
}
