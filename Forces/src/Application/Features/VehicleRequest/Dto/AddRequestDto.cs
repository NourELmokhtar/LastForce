using Forces.Application.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.VehicleRequest.Dto
{
    public class AddRequestDto
    {
        public int Id { get; set; }
        public List<VehicleRequestPackageDto> Packages { get; set; }
        [Required]
        public DateTime? BookingDateTime { get; set; }
        [Required]
        public DateTime? EndDateTime { get; set; }
        [Required]
        public ShipmentType ShipmentType { get; set; }
        public int? PassengersCount { get; set; }
        public Genders? PassengersGender { get; set; }
        [Required]
        public bool WithDriver { get; set; }
        public string RequestNote { get; set; }
        public string KindJob { get; set; }
        public bool PublicRequest { get; set; }
        [Required]
        public int PickupBaseId { get; set; }
        [Required]
        public int DropBaseId { get; set; }

    }
    public class VehicleRequestPackageDto
    {
        public int Id { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public decimal? Weight { get; set; }
        [Required]
        public int PickupBaseId { get; set; }
        [Required]
        public int DropBaseId { get; set; }
        public string PackageNote { get; set; }
        public string AthurityCode { get; set; }
    }

}
