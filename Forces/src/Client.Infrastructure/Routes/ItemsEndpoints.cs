using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public static class ItemsEndpoints
    {
        public static string Save = "api/v1/Items";
        public static string GetAll = "api/v1/Items";
        public static string GetAllByConditions = "api/v1/Items/Get";
        public static string Delete(int Id) => $"api/v1/Items/{Id}";

    }
}
