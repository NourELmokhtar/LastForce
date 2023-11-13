using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.House.Queries.GetBySpecifics
{
    public class GetHouseByResponse
    {
        public int Id { get; set; }
        public string HouseName { get; set; }
        public string HouseCode { get; set; }
        public int BaseId { get; set; }
    }
}
