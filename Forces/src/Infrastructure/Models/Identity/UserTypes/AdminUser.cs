using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Models.Identity.UserTypes
{
    public class AdminUser : Appuser
    {
        public bool IsAdmin { get; set; } = true;
    }
}
