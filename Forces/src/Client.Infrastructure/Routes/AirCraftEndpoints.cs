using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public static class AirCraftEndpoints
    {
        public static string GetAll => "api/v1/AirCraft";
        public static string Save => "api/v1/AirCraft";
        public static string GetById(int Id) => $"api/v1/AirCraft/{Id}";
        public static string GetByKindId(int id) => $"api/v1/AirCraft/Kind/{id}";
        public static string Delete(int id) => $"api/v1/AirCraft/{id}";
    }
}
