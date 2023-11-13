using Forces.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.Building.Queries.GetAll
{
    public class GetAllBuildingsResponse
    {
        public int Id { get; set; }
        public string BuildingName { get; set; }
        public string BuildingCode { get; set; }
        public string BaseName { get; set; }

    }


}
