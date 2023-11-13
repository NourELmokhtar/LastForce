using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.BaseSections.Queries.GetByBaseId
{
    public class GetAllSectionsByBaseIdQueryResponse
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public string SectionCode { get; set; }
        public int BaseId { get; set; }
        public int ForceId { get; set; }
    }
}
