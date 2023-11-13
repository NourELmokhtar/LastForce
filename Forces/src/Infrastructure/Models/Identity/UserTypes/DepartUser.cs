using Forces.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Models.Identity.UserTypes
{
    public class DepartUser : OCLogUser
    {
        public int DepartID { get; set; }
        public DepartType DepartType { get; set; }
        public bool IsOfficer { get; set; }

    }
}
