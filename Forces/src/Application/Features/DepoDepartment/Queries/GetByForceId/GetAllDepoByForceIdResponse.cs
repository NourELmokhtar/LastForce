using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.DepoDepartment.Queries.GetByForceId
{
    public class GetAllDepoByForceIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ForceID { get; set; }
    }
}
