using Forces.Application.Interfaces.Services;
using Forces.Infrastructure.Contexts;
using Forces.Infrastructure.Helpers;
using Forces.Infrastructure.Models.Identity;
using Forces.Shared.Constants.Notification;
using Forces.Shared.Constants.Permission;
using Forces.Shared.Constants.Role;
using Forces.Shared.Constants.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Forces.Infrastructure
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly ILogger<DatabaseSeeder> _logger;
        private readonly IStringLocalizer<DatabaseSeeder> _localizer;
        private readonly ForcesDbContext _db;
        private readonly UserManager<Appuser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public DatabaseSeeder(
            UserManager<Appuser> userManager,
            RoleManager<AppRole> roleManager,
            ForcesDbContext db,
            ILogger<DatabaseSeeder> logger,
            IStringLocalizer<DatabaseSeeder> localizer)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _logger = logger;
            _localizer = localizer;
        }

        public void Initialize()
        {
            AddAdministrator();
            AddBasicUser();
            AddForce();
            AddBase();
            AddBaseSection();
            _db.SaveChanges();
        }
        private void AddForce()
        {

            Task.Run(async () =>
            {

                if (!await _db.Forces.AnyAsync())
                {
                    Application.Models.Forces force = new Application.Models.Forces()
                    {
                        ForceName = "Force 1",
                        ForceCode = "FORCE1"
                    };
                    Application.Models.Forces force2 = new Application.Models.Forces()
                    {
                        ForceName = "Force 2",
                        ForceCode = "FORCE2"
                    };
                    _db.Forces.Add(force);
                    _db.Forces.Add(force2);
                    await _db.SaveChangesAsync();
                }

            }).GetAwaiter().GetResult();


        }
        private void AddBase()
        {
            Task.Run(async () =>
            {
                if (!await _db.Bases.AnyAsync())
                {
                    Application.Models.Bases _base = new Application.Models.Bases()
                    {
                        BaseName = "BASE 1",
                        BaseCode = "BASE1",
                        ForceId = 1
                    };
                    Application.Models.Bases _base2 = new Application.Models.Bases()
                    {
                        BaseName = "BASE 2",
                        BaseCode = "BASE2",
                        ForceId = 2
                    };
                    _db.Bases.Add(_base);
                    _db.Bases.Add(_base2);
                    await _db.SaveChangesAsync();
                }

            }).GetAwaiter().GetResult();

        }
        private void AddBaseSection()
        {
            Task.Run(async () =>
            {
                if (!await _db.BasesSections.AnyAsync())
                {
                    Application.Models.BasesSections _baseSection = new Application.Models.BasesSections()
                    {
                        SectionName = "BASE Section 1",
                        SectionCode = "BASESECTION1",
                        BaseId = 1,
                    };
                    Application.Models.BasesSections _baseSection2 = new Application.Models.BasesSections()
                    {
                        SectionName = "BASE Section 2",
                        SectionCode = "BASESECTION2",
                        BaseId = 2,
                    };
                    _db.BasesSections.Add(_baseSection);
                    _db.BasesSections.Add(_baseSection2);
                    await _db.SaveChangesAsync();
                }

            }).GetAwaiter().GetResult();
        }

        private void AddForceAdminUser()
        {

        }
        private void AddBaseAdminUser()
        {

        }
        private void AddAdministrator()
        {
            Task.Run(async () =>
            {
                //Check if AppRole Exists
                var adminRole = new AppRole(RoleConstants.AdministratorRole, _localizer["Administrator role with full permissions"]);
                var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                    _logger.LogInformation(_localizer["Seeded Administrator AppRole."]);
                }
                //Check if User Exists
                var superUser = new Appuser
                {
                    FirstName = "Mohamed",
                    LastName = "Khalif",
                    Email = "Admin@Admin.com",
                    UserName = "Admin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedOn = DateTime.Now,
                    IsActive = true,
                    UserType = Application.Enums.UserType.SuperAdmin,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
                if (superUserInDb == null)
                {
                    //Default Password Var UserConstants.DefaultPassword
                    await _userManager.CreateAsync(superUser, UserConstants.DefaultPassword);
                    var result = await _userManager.AddToRoleAsync(superUser, RoleConstants.AdministratorRole);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(_localizer["Seeded Default SuperAdmin User."]);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }
                foreach (var permission in Permissions.GetRegisteredPermissions())
                {
                    await _roleManager.AddPermissionClaim(adminRoleInDb, permission);
                }
                //foreach (var Entity in NotificationsEntities.GetRegisteredEntities())
                //{
                //    await _userManager.AddClaimAsync(superUser, new System.Security.Claims.Claim(ApplicationClaimTypes.NotificationsEntities, Entity));

                //}

            }).GetAwaiter().GetResult();

        }

        private void AddBasicUser()
        {
            Task.Run(async () =>
            {
                //Check if AppRole Exists
                var basicRole = new AppRole(RoleConstants.BasicRole, _localizer["Basic role with default permissions"]);
                var basicRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
                if (basicRoleInDb == null)
                {
                    await _roleManager.CreateAsync(basicRole);
                    _logger.LogInformation(_localizer["Seeded Basic AppRole."]);
                }
                //Check if User Exists
                var basicUser = new Appuser
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@blazorhero.com",
                    UserName = "johndoe",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedOn = DateTime.Now,
                    IsActive = true
                };
                var basicUserInDb = await _userManager.FindByEmailAsync(basicUser.Email);
                if (basicUserInDb == null)
                {
                    await _userManager.CreateAsync(basicUser, UserConstants.DefaultPassword);
                    await _userManager.AddToRoleAsync(basicUser, RoleConstants.BasicRole);
                    _logger.LogInformation(_localizer["Seeded User with Basic AppRole."]);
                }
            }).GetAwaiter().GetResult();
        }
    }
}