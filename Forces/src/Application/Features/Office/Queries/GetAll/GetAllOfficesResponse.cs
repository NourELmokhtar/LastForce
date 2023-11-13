using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.Office.Queries.GetAll
{
    public class GetAllOfficeResponse
    {
        public int Id { get; set; }
        public string OfficeName { get; set; }
      
        public int BasesSectionsId { get; set; }
    }
}
