using Forces.Application.Models;
using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application
{
    public class VehicleRequestStopStation : AuditableEntity<int>
    {
        public VehicleRequestStopStation()
        {
            Packages = new HashSet<VehicleRequestPackage>();
        }
        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        public virtual VehicleRequest VehicleRequest { get; set; }
        public virtual ICollection<VehicleRequestPackage> Packages { get; set; }
        public int BaseId { get; set; }
        [ForeignKey("BaseId")]
        public virtual Bases DropBase { get; set; }
        public int Order { get; set; }
        public string StopNote { get; set; }
    }
}
