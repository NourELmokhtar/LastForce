using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public static class AirKindEndpoints
    {
        public static string GetAll => "api/v1/AirKind";
        public static string GetAlll => "api/v1/AirKind/GetAllAirKind";
        public static string Save => "api/v1/AirKind";
        public static string GetById(int Id) => $"api/v1/AirKind/{Id}";
        public static string GetByTypeId(int id) => $"api/v1/AirKind/AirType/{id}";
        public static string Delete(int id) => $"api/v1/AirKind/{id}";
    }
}
