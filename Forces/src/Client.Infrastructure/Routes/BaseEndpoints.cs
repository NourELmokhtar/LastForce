using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public class BaseEndpoints
    {
        public static string Save = "api/v1/Bases";
        public static string GetAll = "api/v1/Bases";
        public static string GetAllBases = "api/v1/Bases/GetAllBases";
        public static string Delete(int Id) => $"api/v1/Bases/{Id}";
        public static string GetBaseById(int Id) => $"api/v1/Bases/{Id}";
        public static string GetBaseByForceId(int Id) => $"api/v1/Bases/force/{Id}";

    }
}
