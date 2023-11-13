using Forces.Application.Features.BaseSections.Queries.GetByBaseId;
using Forces.Application.Features.Forces.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.Bases.Queries.GetAll
{
    public class GetAllBasesResponse
    {
        public int Id { get; set; }
        public string BaseName { get; set; }
        public string BaseCode { get; set; }
        public int ForceId { get; set; }
        public GetAllForcesResponse Force { get; set; }
        public List<GetAllSectionsByBaseIdQueryResponse> Sections { get; set; }
    }
}
