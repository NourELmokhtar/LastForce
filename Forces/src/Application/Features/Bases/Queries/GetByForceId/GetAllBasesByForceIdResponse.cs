using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.Bases.Queries.GetByForceId
{
    public class GetAllBasesByForceIdResponse
    {
        public int Id { get; set; }
        public string BaseName { get; set; }
        public string BaseCode { get; set; }
        public int ForceId { get; set; }
    }
}
