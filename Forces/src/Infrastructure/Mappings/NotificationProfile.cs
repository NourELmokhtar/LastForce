using AutoMapper;
using Forces.Application.Responses.Identity;
using Forces.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Mappings
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<NotificationResponse, NotificationsUsers>().ReverseMap();
        }
    }
}
