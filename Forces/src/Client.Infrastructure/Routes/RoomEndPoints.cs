using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public class RoomEndPoints
    {
        public static string Save = "api/Room";
        public static string GetAll = "api/Room";
        public static string GetAllRooms = "api/Room/GetAllRooms";
        public static string Delete(int Id) => $"api/Room/{Id}";
        public static string GetRoomById(int Id) => $"api/Room/{Id}";
    }
}
