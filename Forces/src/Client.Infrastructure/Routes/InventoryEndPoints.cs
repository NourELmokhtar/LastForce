using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    internal class InventoryEndPoints
    {
        public static string Save = "api/v1/Inventory";
        public static string GetAll = "api/v1/Inventory";
        public static string GetAllInventories = "api/v1/Inventory/GetAllInventories";
        public static string Delete(int Id) => $"api/v1/Inventory/{Id}";
        public static string GetInventoryById(int Id) => $"api/v1/Inventory/{Id}";
    }
}
