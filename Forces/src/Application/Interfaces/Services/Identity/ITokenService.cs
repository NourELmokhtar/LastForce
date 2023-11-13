using Forces.Application.Interfaces.Common;
using Forces.Application.Requests.Identity;
using Forces.Application.Responses.Identity;
using Forces.Shared.Wrapper;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Services.Identity
{
    public interface ITokenService : IService
    {
        Task<Result<TokenResponse>> LoginAsync(TokenRequest model);

        Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
    }
}