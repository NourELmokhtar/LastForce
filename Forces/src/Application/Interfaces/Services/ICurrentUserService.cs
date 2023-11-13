using Forces.Application.Enums;
using Forces.Application.Interfaces.Common;

namespace Forces.Application.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}