using Forces.Application.Enums;
using Forces.Application.Interfaces.Services;
using Forces.Infrastructure.Contexts;
using Forces.Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Forces.Server.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Claims = httpContextAccessor.HttpContext?.User?.Claims.AsEnumerable().Select(item => new KeyValuePair<string, string>(item.Type, item.Value)).ToList();
        }

        public string UserId { get; }
        public Appuser AppUser { get; set; }
        public List<KeyValuePair<string, string>> Claims { get; set; }

    }
}