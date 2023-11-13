

using Forces.Application.Enums;
using Forces.Application.Models;
using Forces.Domain.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forces.Application.Models

{
    public class Service : AuditableEntity<int>
    {
        public string ServiceID { get; set; }
        public string ServicePlace { get; set; }
        public int ServicePlaceID { get; set; }
        [ForeignKey("ServicePlaceID")]
        public virtual ServicePlaceNames ServicePlaceName { get; set; }
        public DateTime? DateOfServic { get; set; }
       

    }
}
