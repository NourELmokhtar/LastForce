using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.PersonalItemOperations.Commands.AddEdit
{
    public class OperationDetailsDto
    {
        public int Id { get; set; }
        public int PersonalItemId { get; set; }
        public DateTime OperationDate { get; set; }
        public string UserId { get; set; }
        public int Qty { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalLinePrice { get; set; }
    }
}
