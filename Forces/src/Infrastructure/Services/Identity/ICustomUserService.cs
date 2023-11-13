using Forces.Application.Interfaces.Common;
using Forces.Infrastructure.Contexts;
using Forces.Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Services.Identity
{
    public class ICustomUserService<TUser, TRole> : IService
        where TUser : Appuser
        where TRole : AppRole
    {
        private readonly ForcesDbContext _context;
        private readonly UserManager<TUser> _userManager;
        private readonly RoleManager<TRole> _roleManager;

        public ICustomUserService(ForcesDbContext context, UserManager<TUser> userManager, RoleManager<TRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<List<TUser>> GetAll()
        {
            return await _context.Set<TUser>().ToListAsync();
        }
        public async Task<int> AddUser(TUser userModel)
        {
            await _context.Set<TUser>().AddAsync(userModel);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateUser(TUser userModel)
        {
            var user = await _context.Set<TUser>().FirstOrDefaultAsync(u => u.Id == userModel.Id);

            return await _context.SaveChangesAsync();
        }
    }
}
