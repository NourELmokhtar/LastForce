using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.Inventory.Queries.GetAll
{
    public class GetAllInventoriesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? RoomName { get; set; }
        public string? HouseName { get; set; }
        public string? BaseSectionName { get; set; }

    }
}
