using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public class BuildingEndPoints
    {
        public static string Save = "api/Building";
        public static string GetAll = "api/Building";
        public static string GetAllBuildings = "api/Building/GetAllBuildings";
        public static string Delete(int Id) => $"api/Building/{Id}";
        public static string GetBuildingById(int Id) => $"api/Building/{Id}";
    }
}
