using Forces.Application.Enums;
using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Services
{
    public interface INotificationService<T,TNotifire>
        where T : AuditableEntity<int>
        where TNotifire : INotifire<T>
    {
     
        public Task NotifyOwner(T entity , TNotifire notifire);
        public Task NotifySteppers(T entity, TNotifire notifire);

    }
}
