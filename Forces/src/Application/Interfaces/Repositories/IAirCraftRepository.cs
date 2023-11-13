using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IAirCraftRepository
    {
        Task<bool> IsAirCraftInused(int AirKindId);
        Task<bool> IsNameExist(string AirCraftName, int AirKindId);
        Task<bool> IsCodeExist(int airCraftCode);
    }
}
