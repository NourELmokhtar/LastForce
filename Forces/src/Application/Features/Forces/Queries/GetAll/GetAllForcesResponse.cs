using Forces.Application.Features.Bases.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.Forces.Queries.GetAll
{
    public class GetAllForcesResponse
    {
        public int Id { get; set; }
        public string ForceName { get; set; }
        public string ForceCode { get; set; }

    }
}
