using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public static class AirTypeEndpoints
    {
        public static string Save => "api/v1/AirType";
        public static string Delete(int Id) => $"api/v1/AirType/{Id}";
        public static string GetById(int id) => $"api/v1/AirType/{id}";
        public static string GetAll => $"api/v1/AirType/GetAllAirType";
    }
}
