using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public class DepoEndpoints
    {
        public static string Save = "api/v1/Depo";
        public static string GetAll = "api/v1/Depo";
        public static string GetAllDepos = "api/v1/Depo/GetAllDepos";
        public static string Delete(int Id) => $"api/v1/Depo/{Id}";
        public static string GetByForceId(int Id) => $"api/v1/Depo/Force/{Id}";
    }
}
