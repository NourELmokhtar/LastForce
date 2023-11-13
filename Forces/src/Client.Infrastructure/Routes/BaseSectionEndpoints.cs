using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public class BaseSectionEndpoints
    {
        public static string Save = "api/v1/BaseSections";
        public static string GetAll = "api/v1/BaseSections";
        public static string GetAllSections = "api/v1/BaseSections/GetAllBaseSections";
        public static string Delete(int Id) => $"api/v1/BaseSections/{Id}";
        public static string GetBaseSectionById(int Id) => $"api/v1/BaseSections/{Id}";
        public static string GetSectionByBaseId(int Id) => $"api/v1/BaseSections/Base/{Id}";
    }
}
