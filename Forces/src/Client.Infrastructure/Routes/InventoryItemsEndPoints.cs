using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public class InventoryItemEndPoints
    {
        public static string Save = "api/InventoryItem";
        public static string GetAll = "api/InventoryItem";
        public static string GetAllInventoryItems = "api/InventoryItem/GetAllInventoryItems";
        public static string Delete(int Id) => $"api/InventoryItem/{Id}";
        public static string GetInventoryItemById(int Id) => $"api/InventoryItem/{Id}";
    }
}
