using Forces.Application.Interfaces.Repositories;
using Forces.Application.Models;
using Forces.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Repositories
{
    public class VoteCodeRepository : IVoteCodeRepository
    {
        private protected readonly IRepositoryAsync<VoteCodes, int> _repository;

        public VoteCodeRepository(IRepositoryAsync<VoteCodes, int> repository)
        {
            _repository = repository;
        }

        public async Task<Dictionary<int, string>> GetAllVoteCodesAsync()
        {
            Dictionary<int,string>Codes = new Dictionary<int,string>();
            var votecodes = await _repository.GetAllAsync();
            foreach ( var votecode in votecodes )
            {
                Codes[votecode.Id] = votecode.VoteCode;
            }
            return Codes;
            
        }

        public async Task<string> GetVoteCodeByIdAsync(int Id)
        {
           return (await _repository.GetByIdAsync(Id)).VoteCode;
        }

        public Task<bool> isCodeExist(string Code)
        {
            throw new NotImplementedException();
        }

        public Task<bool> isShortcutExist(string Shortcut)
        {
            throw new NotImplementedException();
        }
    }
}
