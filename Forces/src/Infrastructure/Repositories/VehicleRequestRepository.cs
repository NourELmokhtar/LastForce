using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Application.Models;
using Forces.Infrastructure.Contexts;
using Forces.Infrastructure.Models.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Repositories
{
    public class VehicleRequestRepository : IVehicleRequestRepository
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ForcesDbContext _dbContext;
        private Appuser CurrentUser;

        public VehicleRequestRepository(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService, ForcesDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _dbContext = dbContext;
            CurrentUser = _dbContext.Users.FirstOrDefault(x=>x.Id == _currentUserService.UserId);
        }

        public Task<string> GenerateAuthCodeAsync()
        {
            return Task.FromResult<string>(string.Empty);
        }

        public async Task<string> GenerateRequestCodeAsync()
        {
            var Count = await _unitOfWork.Repository<VehicleRequest>().Entities.Where(x => x.CreatedOn.Year == DateTime.Now.Year && x.BaseId == CurrentUser.BaseID).CountAsync();
            Count++;
            var Number = Count.ToString($"d6");// 000011
            return $"BV/{Number}/{DateTime.Now.ToString("yy")}";
        }
    }
}
