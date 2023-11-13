using Forces.Application.Features.Forces.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.Bases.Queries.GetById
{
    public class GetBaseByIdResponse
    {
        public int Id { get; set; }
        public string BaseName { get; set; }
        public string BaseCode { get; set; }
        public int ForceId { get; set; }
        public GetForceByIdResponse Force { get; set; }
    }
}
