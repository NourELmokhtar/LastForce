using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Domain.Contracts
{
    public interface INotifire<T>
        where T : AuditableEntity<int>
    {
        public T Entity { get; set; }
        public Type StateEnum { get; set; }
        public string Message { get; set; }
        public string ReturnURL { get; set; }
    }
}
