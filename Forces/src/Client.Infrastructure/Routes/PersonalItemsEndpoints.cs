using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public class PersonalItemsEndpoints
    {
        public static string Save = "api/v1/PersonalItems";
        public static string GetAll = "api/v1/PersonalItems";
        public static string GetByCondition = "api/v1/PersonalItems/Get";
        public static string Delete(int Id) => $"api/v1/PersonalItems/{Id}";
        public static string GetById(int Id) => $"api/v1/PersonalItems/Get/{Id}";

    }
}
