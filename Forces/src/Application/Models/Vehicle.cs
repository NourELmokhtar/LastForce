using Forces.Application.Enums;
using Forces.Domain.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forces.Application.Models
{
    public class Vehicle : AuditableEntity<int>
    {

        public string VehicleNumber { get; set; }
        public string VehicleName { get; set; }
        public int ColorID { get; set; }
        [ForeignKey("ColorID")]
        public virtual Color VehicleColor { get; set; }
        public string Year { get; set; } // true , False
        public string MadeIn { get; set; }
        public BattryType BattryType { get; set; }
        public string EngineNo { get; set; }
        public DateTime? DateOfEnter { get; set; }
        public DateTime? EndOfServiceDate { get; set; }
        public int WheelsCount { get; set; }
        public string WheelsSize { get; set; }
        public FuleType FuleType { get; set; }
        public string AdditionalNumber { get; set; }
        public string WheelsYear { get; set; }
        public string VIN { get; set; }
        public VehicleState State { get; set; }
        public VehicleCarryType VehicleCarryType { get; set; }
    }
}