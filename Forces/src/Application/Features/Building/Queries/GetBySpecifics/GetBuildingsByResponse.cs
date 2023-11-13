using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.Building.Queries.GetBySpecifics
{
    internal class GetBuildingsByResponse
    {
        public int Id { get; set; }
        public string BuildingName { get; set; }
        public string BuildingCode { get; set; }
        public int BaseId { get; set; }
    }
}
