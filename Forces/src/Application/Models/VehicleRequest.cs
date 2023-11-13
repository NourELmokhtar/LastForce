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
    public class VehicleRequest : AuditableEntity<int>
    {
        public VehicleRequest()
        {
            Packages = new HashSet<VehicleRequestPackage>();
            StopStations = new HashSet<VehicleRequestStopStation>();
        }
        public VehicleRequestSteps RequestStep { get; set; }
        public virtual ICollection<VehicleRequestPackage> Packages { get; set; }
        public string RequestCode { get; set; }
        public DateTime BookingDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public ShipmentType ShipmentType { get; set; }
        public int? PassengersCount { get; set; }
        public Genders? PassengersGender { get; set; }
        public int? BaseId { get; set; }
        public int? ForceId { get; set; }
        public bool WithDriver { get; set; }
        public string RequestNote { get; set; }
        public string KindJob { get; set; }
        public bool PublicRequest { get; set; }
        public bool IsDone { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public int PickupBaseId { get; set; }
        public RequestState RequestState { get; set; }
        public int DropBaseId { get; set; }
        [ForeignKey("PickupBaseId")]
        public virtual Bases PickupBase { get; set; }
        [ForeignKey("DropBaseId")]
        public virtual Bases DropBase { get; set; }
        [ForeignKey("BaseId")]
        public virtual Bases Base { get; set; }
        public virtual ICollection<VehicleRequestStopStation> StopStations { get; set; }
    }
}
