using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IAirKindRepository
    {
        Task<bool> IsNameExist(string AirKindName, int? Id = null);
        Task<bool> IsCodeExist(string AirKindCode, int? Id = null );
        Task<bool> IsAirKindInused(int Id);
    }
}
