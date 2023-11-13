using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public class InventoryItemBridgeEndPoints
    {
        public static string Save = "api/InventoryItemBridge";
        public static string GetAll = "api/InventoryItemBridge";
        public static string GetAllByConditions = "api/InventoryItemBridge/Get";
        public static string Delete(int Id) => $"api/InventoryItemBridge/{Id}";
    }
}
