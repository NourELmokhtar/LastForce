using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public class PersonEndPoints
    {
        public static string Save = "api/Person";
        public static string GetAll = "api/Person";
        public static string GetAllPersons = "api/Person/GetAllPersons";
        public static string Delete(int Id) => $"api/Person/{Id}";
        public static string GetPersonById(int Id) => $"api/Person/{Id}";
    }
}
