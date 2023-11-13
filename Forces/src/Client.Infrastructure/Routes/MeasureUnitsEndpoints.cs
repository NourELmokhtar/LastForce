using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public static class MeasureUnitsEndpoints
    {
        public static string Save = "api/v1/MeasureUnits";
        public static string GetAll = "api/v1/MeasureUnits";
        public static string Delete(int Id) => $"api/v1/MeasureUnits/{Id}";
    }
}
