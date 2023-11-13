using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public class HQEndpoints
    {
        public static string Save = "api/v1/HQ";
        public static string GetAll = "api/v1/HQ";
        public static string GetAllHQ = "api/v1/HQ/GetAllHQs";
        public static string Delete(int Id) => $"api/v1/HQ/{Id}";
        public static string GetByForceId(int Id) => $"api/v1/HQ/Force/{Id}";
    }
}
