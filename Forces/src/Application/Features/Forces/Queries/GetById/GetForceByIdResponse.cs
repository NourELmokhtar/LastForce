using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.Forces.Queries.GetById
{
    public class GetForceByIdResponse
    {
        public int Id { get; set; }
        public string ForceName { get; set; }
        public string ForceCode { get; set; }
    }
}
