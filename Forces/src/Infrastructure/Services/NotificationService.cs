using Forces.Application.Enums;
using Forces.Application.Extensions;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Application.Models;
using Forces.Domain.Contracts;
using Forces.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Services
{
    public class NotificationService<T,TNotifire> : INotificationService<T, TNotifire>
        where T : AuditableEntity<int>
        where TNotifire : INotifire<T>
    {
        private protected IUnitOfWork<int> _unitOfWork;

        public NotificationService(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task NotifyOwner(T entity, TNotifire notifire)
        {
            var Existnotification = await _unitOfWork.Repository<NotificationsUsers>().Entities.FirstOrDefaultAsync(x => x.TargetUserId == entity.CreatedBy && x.EntityPrimaryKey == entity.Id.ToString() && x.NotificationType == typeof(T).Name);
            if (Existnotification != null)
            {
                Existnotification.Readed = false;
                Existnotification.Seen = false;
                Existnotification.Title = notifire.Message;
                Existnotification.Description = $"Your Request State is {((Enum)GetEntityState(entity, notifire)).ToDescriptionString()}";
                Existnotification.CreatedOn = DateTime.Now;
                await _unitOfWork.Repository<NotificationsUsers>().UpdateAsync(Existnotification);
            }
            else
            {

                NotificationsUsers notification = new NotificationsUsers();
                notification.EntityPrimaryKey = entity.Id.ToString();
                notification.NotificationType = typeof(T).Name;
                notification.TargetUserId = entity.CreatedBy;
                notification.RefUrl = notifire.ReturnURL;
                notification.Readed = false;
                notification.Seen = false;
                notification.Title = notifire.Message;
                notification.Description = $"Your Request State is {((Enum)GetEntityState(entity, notifire)).ToDescriptionString()}";
                await _unitOfWork.Repository<NotificationsUsers>().AddAsync(notification);
            }
            await _unitOfWork.Commit(new System.Threading.CancellationToken());
        }


        public Task NotifySteppers(T entity, TNotifire notifire)
        {
            throw new NotImplementedException();
        }

        private object GetEntityState(T entity, TNotifire notifire)
        {
           
            foreach (PropertyInfo propertyInfo in entity.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType == notifire.StateEnum)
                {
                    Enum value = (Enum)Enum.Parse(notifire.StateEnum, propertyInfo.GetValue(entity, null).ToString());
                    return value;
                }
            }
            return Activator.CreateInstance(notifire.StateEnum);
        }

    }
}
