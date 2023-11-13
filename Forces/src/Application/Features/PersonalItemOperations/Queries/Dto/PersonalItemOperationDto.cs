using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.PersonalItemOperations.Queries.Dto
{
    public class PersonalItemOperationDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OperationDate { get; set; }
        public decimal Total { get; set; }
        public int ForceId { get; set; }
        public int? BaseId { get; set; }
        public int? BaseSectionId { get; set; }
        public List<PersonalItemOperationDetail> Details { get; set; }
    }
    public class PersonalItemOperationDetail
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
