using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Models.Identity.UserTypes
{
    public class OCLogUser : BasicUser
    {
        public string JobTitle { get; set; }
        public int? Rank { get; set; }

    }
}
