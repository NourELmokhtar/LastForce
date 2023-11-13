using AutoMapper;
using Forces.Application.Interfaces.Chat;
using Forces.Application.Models.Chat;
using Forces.Infrastructure.Models.Identity;

namespace Forces.Infrastructure.Mappings
{
    public class ChatHistoryProfile : Profile
    {
        public ChatHistoryProfile()
        {
            CreateMap<ChatHistory<IChatUser>, ChatHistory<Appuser>>().ReverseMap();
        }
    }
}