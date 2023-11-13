using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class VehicleRequestPackage  : AuditableEntity<int>
    {
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public decimal? Weight { get; set; }
        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        public virtual VehicleRequest VehicleRequest { get; set; }
        public string PickupLocation { get; set; }
        public string DropLocation { get; set; }
        public int PickupBaseId { get; set; }
        public int DropBaseId { get; set; }
        [ForeignKey("PickupBaseId")]
        public virtual Bases PickupBase { get; set; }
        [ForeignKey("DropBaseId")]
        public virtual Bases DropBase { get; set; }
        public string PackageNote { get; set; }
        public string AthurityCode { get; set; }
    }
}
