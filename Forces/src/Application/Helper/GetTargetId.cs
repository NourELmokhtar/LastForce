using Forces.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Helper
{
    public static class GetTargetId

    {
        public static async Task<string> GetNextAvilableId<T>(this IQueryable<T> entitie, int Digits = 6) where T : AuditableEntity<int>
        {
            var Count = await entitie.Where(x => x.CreatedOn.Year == DateTime.Now.Year).CountAsync();
            Count++;
            return Count.ToString($"d{Digits}");
        }

    }
}
