
using Forces.Domain.Contracts;

namespace Forces.Application.Models
{
    public class ServicePlace : AuditableEntity<int>

    {
        public string ServicePlaceID { get; set; }
        public string ServicePlaceName { get; set; }
    }
}
