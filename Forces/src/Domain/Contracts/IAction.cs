using Forces.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Domain.Contracts
{
    public interface IAction<TActions, TResult>
         where TActions : Enum
        where TResult : class
    {
        public Task<TResult> Submit(params RedirectModel[] redirectTo);
        public Task<TResult> Scale();
        public Task<TResult> Reject(params RedirectModel[] redirectTo);
        public Task<TResult> Redirect(params RedirectModel[] redirectTo);

    }
}
