using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Domain.Contracts
{
    public interface IScopedEntity
    {
        public int? ForceID { get; set; }
        public int? BaseID { get; set; }
        public int? BaseSectionID { get; set; }
    }
}
