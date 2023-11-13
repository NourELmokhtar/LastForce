using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.Room.Queries.GetBySpecifics
{
    public class GetRoomByResponse
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public int BuildingId { get; set; }
    }
}
