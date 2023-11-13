using Forces.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.InventoryItem.Queries.GetAll
{
    public class GetAllInventoryItemsResponse
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemArName { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string ItemNsn { get; set; }
        public int MeasureUnitId { get; set; }
        public string MeasureName { get; set; }
        public ItemClass ItemClass { get; set; }
        public int InventoryId { get; set; }
        public DateTime? DateOfEnter { get; set; }
        public DateTime? FirstUseDate { get; set; }
        public DateTime? EndOfServiceDate { get; set; }
        public string SerialNumber { get; set; }
    }
    public class MeasureUnitsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
