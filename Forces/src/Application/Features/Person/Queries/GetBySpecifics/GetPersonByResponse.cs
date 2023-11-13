using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.Person.Queries.GetBySpecifics
{
    public class GetPersonByResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NationalNumber { get; set; }
        public int RoomId { get; set; }
        public string OfficePhone { get; set; }
        public string Phone { get; set; }
        public string Section { get; set; }
    }
}
