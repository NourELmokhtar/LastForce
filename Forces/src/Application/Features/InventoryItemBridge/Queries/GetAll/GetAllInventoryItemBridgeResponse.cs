using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.InventoryItemBridge.Queries.GetAll
{
    public class GetAllInventoryItemBridgeResponse
    {
        public GetAllInventoryItemBridgeResponse() { }
        public int Id { get; set; }
        public string InventoryName { get; set; }
        public string InventoryItemName{ get; set; }
        public string SerialNumber { get; set; }
        public DateTime DateOfEnter { get; set; }

        public static implicit operator List<object>(GetAllInventoryItemBridgeResponse v)
        {
            throw new NotImplementedException();
        }
    }
}
