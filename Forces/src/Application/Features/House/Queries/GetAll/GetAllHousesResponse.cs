using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.House.Queries.GetAll
{
    public class GetAllHousesResponse
    {
        public int Id { get; set; }
        public string HouseName { get; set; }
        public string HouseCode { get; set; }
        public string BaseName { get; set; }
    }
}
