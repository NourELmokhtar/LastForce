using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public class OfficeEndPoints
    {
        public static string Save = "api/Office";
        public static string GetAll = "api/Office";
        public static string GetAllOffices = "api/Office/GetAllOffices";
        public static string Delete(int Id) => $"api/Office/{Id}";
        public static string GetOfficeById(int Id) => $"api/Office/{Id}";
    }
}
