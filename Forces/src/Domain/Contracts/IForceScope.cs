using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Domain.Contracts
{
    public interface IForceScope
    {
        public int ForceId { get; set; }
    }
    public interface IBaseScope
    {
        public int BaseId { get; set; }
    }
    public interface IBaseSectionScope
    {
        public int BaseSectionId { get; set; }
    }
}
