using Forces.Application.Requests.Identity;
using Forces.Application.Responses.Identity;
using Forces.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Identity.RoleClaims
{
    public interface IRoleClaimManager : IManager
    {
        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsAsync();

        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsByRoleIdAsync(string roleId);

        Task<IResult<string>> SaveAsync(RoleClaimRequest role);

        Task<IResult<string>> DeleteAsync(string id);
    }
}