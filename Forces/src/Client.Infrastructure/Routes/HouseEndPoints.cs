using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public class HouseEndPoints
    {
        public static string Save = "api/House";
        public static string GetAll = "api/House";
        public static string GetAllHouses = "api/House/GetAllHouses";
        public static string Delete(int Id) => $"api/House/{Id}";
        public static string GetHouseById(int Id) => $"api/House/{Id}";
    }
}
