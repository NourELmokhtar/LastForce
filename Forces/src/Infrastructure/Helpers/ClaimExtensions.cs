using Forces.Application.Responses.Identity;
using Forces.Infrastructure.Models.Identity;
using Forces.Shared.Constants.Notification;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Helpers
{
    public static class ClaimsHelper
    {
        public static void GetAllPermissions(this List<RoleClaimResponse> allPermissions)
        {
            var modules = typeof(Permissions).GetNestedTypes();

            foreach (var module in modules)
            {
                var fields = module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                foreach (FieldInfo fi in fields)
                {
                    var propertyValue = fi.GetValue(null);

                    if (propertyValue is not null)
                        allPermissions.Add(new RoleClaimResponse { Value = propertyValue.ToString(), Type = ApplicationClaimTypes.Permission, Group = module.Name });
                    //TODO - take descriptions from description attribute
                }
            }

        }
        public static void GetAllNotificationEntites(this List<RoleClaimResponse> allPermissions)
        {
            var modules = typeof(NotificationsEntities).GetNestedTypes();

            foreach (var module in modules)
            {
                var fields = module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                foreach (FieldInfo fi in fields)
                {
                    var propertyValue = fi.GetValue(null);

                    if (propertyValue is not null)
                        allPermissions.Add(new RoleClaimResponse { Value = propertyValue.ToString(), Type = ApplicationClaimTypes.NotificationsEntities, Group = module.Name });
                    //TODO - take descriptions from description attribute
                }
            }

        }

        public static async Task<IdentityResult> AddPermissionClaim(this RoleManager<AppRole> roleManager, AppRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == ApplicationClaimTypes.Permission && a.Value == permission))
            {
                return await roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, permission));
            }

            return IdentityResult.Failed();
        }
        public static async Task<IdentityResult> AddNotificationClaim(this UserManager<Appuser> userManager, Appuser user, string NotificationEntity)
        {
            var allClaims = await userManager.GetClaimsAsync(user);
            if (!allClaims.Any(a => a.Type == ApplicationClaimTypes.NotificationsEntities && a.Value == NotificationEntity))
            {
                return await userManager.AddClaimAsync(user, new Claim(ApplicationClaimTypes.NotificationsEntities, NotificationEntity));
            }
            return IdentityResult.Failed();
        }
    }
}