using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public static class SectionStoreEndpoints
    {
        public static string Save = "api/v1/SectionStore";
        public static string GetAll = "api/v1/SectionStore";
        public static string Delete(int Id) => $"api/v1/SectionStore/{Id}";
    }
}
