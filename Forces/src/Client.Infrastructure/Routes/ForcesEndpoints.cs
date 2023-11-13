using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public class ForcesEndpoints
    {
        public static string Save = "api/v1/Forces";
        public static string GetAll = "api/v1/Forces";
        public static string GetAllForces = "api/v1/Forces/GetAllForces";
        public static string Delete(int Id) => $"api/v1/Forces/{Id}";
        public static string GetForceById(int Id) => $"api/v1/Forces/{Id}";
    }
}
