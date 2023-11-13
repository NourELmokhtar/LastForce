using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public class PersonalItemsOperationsEndpoints
    {
        public static string Save = "api/v1/PersonalItemsOperations";
        public static string GetAll = "api/v1/PersonalItemsOperations/GetAll";
        public static string GetByCondition = "api/v1/PersonalItemsOperations/Get";
        public static string Check = "api/v1/PersonalItemsOperations/Check";

    }
}
