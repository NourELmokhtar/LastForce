using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IVoteCodeRepository
    {
        Task<bool> isCodeExist(string Code);
        Task<bool> isShortcutExist(string Shortcut);
        Task<string> GetVoteCodeByIdAsync(int Id);
        Task<Dictionary<int, string>> GetAllVoteCodesAsync();
    }
}
