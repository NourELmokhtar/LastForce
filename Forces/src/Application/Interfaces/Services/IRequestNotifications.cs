using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Services
{
    public interface IRequestNotifications<TRequest>

    {
        public Task NotifyOwner(TRequest request);
        public Task NotifySteppers(TRequest request);
        public Task NotifyvCodeControllers(TRequest request);
    }
}
