using AutoMapper;
using Forces.Application.Exceptions;
using Forces.Application.Interfaces.Chat;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Application.Interfaces.Services.Identity;
using Forces.Application.Models.Chat;
using Forces.Application.Responses.Identity;
using Forces.Infrastructure.Contexts;
using Forces.Infrastructure.Models.Identity;
using Forces.Shared.Constants.Role;
using Forces.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Services
{
    public class ChatService : IChatService
    {
        private readonly ForcesDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IStringLocalizer<ChatService> _localizer;


        public ChatService(
            ForcesDbContext context,
            IMapper mapper,
            IUserService userService,

            IStringLocalizer<ChatService> localizer)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _localizer = localizer;

        }

        public async Task<Result<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string userId, string contactId)
        {
            var response = await _userService.GetAsync(userId);
            if (response.Succeeded)
            {
                var user = response.Data;
                var query = await _context.ChatHistories
                    .Where(h => (h.FromUserId == userId && h.ToUserId == contactId) || (h.FromUserId == contactId && h.ToUserId == userId))
                    .OrderBy(a => a.CreatedDate)
                    .Include(a => a.FromUser)
                    .Include(a => a.ToUser)
                    .Select(x => new ChatHistoryResponse
                    {
                        FromUserId = x.FromUserId,
                        FromUserFullName = $"{x.FromUser.FirstName} {x.FromUser.LastName}",
                        Message = x.Message,
                        CreatedDate = x.CreatedDate,
                        Id = x.Id,
                        ToUserId = x.ToUserId,
                        ToUserFullName = $"{x.ToUser.FirstName} {x.ToUser.LastName}",
                        ToUserImageURL = x.ToUser.ProfilePictureDataUrl,
                        FromUserImageURL = x.FromUser.ProfilePictureDataUrl
                    }).ToListAsync();
                return await Result<IEnumerable<ChatHistoryResponse>>.SuccessAsync(query);
            }
            else
            {
                throw new ApiException(_localizer["User Not Found!"]);
            }
        }
        public async Task<Result<IEnumerable<ChatHistoryResponse>>> GetChatHistoryMessagesAsync(string userId)
        {
            var response = await _userService.GetAsync(userId);
            if (response.Succeeded)
            {
                var Ousers = await _context.ChatHistories.Include(x => x.FromUser).Include(x => x.ToUser).Where(x => x.FromUserId == userId || x.ToUserId == userId).ToListAsync();

                var user = response.Data;
                var query = await _context.ChatHistories
                    .Where(h => (h.FromUserId == userId || h.ToUserId == userId))
                    .OrderBy(a => a.CreatedDate)
                    .Include(a => a.FromUser)
                    .Include(a => a.ToUser)
                    .Select(x => new ChatHistoryResponse
                    {
                        FromUserId = x.FromUserId,
                        FromUserFullName = $"{x.FromUser.FirstName} {x.FromUser.LastName}",
                        Message = x.Message,
                        CreatedDate = x.CreatedDate,
                        Id = x.Id,
                        ToUserId = x.ToUserId,
                        ToUserFullName = $"{x.ToUser.FirstName} {x.ToUser.LastName}",
                        ToUserImageURL = x.ToUser.ProfilePictureDataUrl,
                        FromUserImageURL = x.FromUser.ProfilePictureDataUrl
                    }).ToListAsync();
                return await Result<IEnumerable<ChatHistoryResponse>>.SuccessAsync(query);
            }
            else
            {
                throw new ApiException(_localizer["User Not Found!"]);
            }
        }

        public async Task<Result<IEnumerable<ChatUserResponse>>> GetChatUsersAsync(string userId)
        {
            var userRoles = await _userService.GetRolesAsync(userId);
            var userIsAdmin = userRoles.Data?.UserRoles?.Any(x => x.Selected && x.RoleName == RoleConstants.AdministratorRole) == true;
            var allUsers = await _context.Users.Where(user => user.Id != userId && (userIsAdmin || user.IsActive && user.EmailConfirmed)).ToListAsync();
            var chatUsers = _mapper.Map<IEnumerable<ChatUserResponse>>(allUsers);
            return await Result<IEnumerable<ChatUserResponse>>.SuccessAsync(chatUsers);
        }

        public async Task<Result<IEnumerable<ChatUserResponse>>> GetChatOUsersAsync(string userId)
        {
            var q = await (from chatMessage in _context.ChatHistories
                           join FromUser in _context.Users on chatMessage.FromUserId equals FromUser.Id
                           join Touser in _context.Users on chatMessage.ToUserId equals Touser.Id
                           from LastMessage in FromUser.ChatHistoryFromUsers
                           where (chatMessage.ToUserId == userId || chatMessage.FromUserId == userId)
                           select new ChatUserResponse()
                           {
                               Id = FromUser.Id
                           }).Distinct().ToListAsync();
            var Ousers = await _context.Users.Where(x => x.ChatHistoryToUsers.Any(x => x.ToUserId == userId)).ToListAsync();
            var From = await _context.ChatHistories.Where(x => x.ToUserId == userId).Select(x => x.FromUserId).Distinct().ToListAsync();
            var To = await _context.ChatHistories.Where(x => x.FromUserId == userId).Select(x => x.ToUserId).Distinct().ToListAsync();
            var All = From.Union(To);
            var users = await _context.Users.Where(x => All.Contains(x.Id)).ToListAsync();
            var chatUsers = _mapper.Map<IEnumerable<ChatUserResponse>>(users);
            return await Result<IEnumerable<ChatUserResponse>>.SuccessAsync(chatUsers);
        }
        public async Task<Result<List<ChatUserResponse>>> GetAllChatsByUserAsync(string userId)
        {

            var q = await (from chatMessage in _context.ChatHistories
                           join FromUser in _context.Users on chatMessage.FromUserId equals FromUser.Id
                           join Touser in _context.Users on chatMessage.ToUserId equals Touser.Id
                           from LastMessage in FromUser.ChatHistoryFromUsers
                           where (chatMessage.ToUserId == userId || chatMessage.FromUserId == userId)
                           select new ChatUserResponse()
                           {
                               Id = FromUser.Id
                           }).Distinct().ToListAsync();
            var Ousers = await _context.Users.Where(x => x.ChatHistoryToUsers.Any(x => x.ToUserId == userId)).ToListAsync();
            var From = await _context.ChatHistories.Where(x => x.ToUserId == userId).Select(x => x.FromUserId).Distinct().ToListAsync();
            var To = await _context.ChatHistories.Where(x => x.FromUserId == userId).Select(x => x.ToUserId).Distinct().ToListAsync();
            var All = From.Union(To);
            var users = await _context.Users.Where(x => All.Contains(x.Id)).ToListAsync();
            var AllMessages = await _context.ChatHistories.Where(x => x.FromUserId == userId || x.ToUserId == userId).ToListAsync();
            var chatUsers = _mapper.Map<List<ChatUserResponse>>(users);
            foreach (var message in chatUsers)
            {
                var LasMessage = AllMessages.Where(x => x.FromUserId == message.Id || x.ToUserId == message.Id).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                message.lastMessage = LasMessage.Message;
                message.LastMessageDate = LasMessage.CreatedDate;
                message.Read = LasMessage.FromUserId == userId ? true : LasMessage.Readed;
                message.seen = LasMessage.FromUserId == userId ? true : LasMessage.Seen;
            }

            return await Result<List<ChatUserResponse>>.SuccessAsync(chatUsers.OrderByDescending(x => x.LastMessageDate).ToList());
        }
        public async Task<IResult> MarkMessageAsRead(string userId, string TargetUserId)
        {
            var Messages = await _context.ChatHistories.Where(x => x.FromUserId == TargetUserId && x.ToUserId == userId).ToListAsync();
            foreach (var Message in Messages)
            {
                Message.Readed = Message.Seen = true;
            }
            await _context.SaveChangesAsync(userId, new System.Threading.CancellationToken());

            return await Result.SuccessAsync();
        }

        public async Task<IResult> MarkAllMessageAsRead(string userId)
        {
            var Messages = await _context.ChatHistories.Where(x => x.ToUserId == userId).ToListAsync();
            foreach (var Message in Messages)
            {
                Message.Readed = Message.Seen = true;
            }
            await _context.SaveChangesAsync(userId, new System.Threading.CancellationToken());
            return await Result.SuccessAsync();
        }
        public async Task<IResult> MarkAllMessageAsSeen(string userId)
        {
            var Messages = await _context.ChatHistories.Where(x => x.ToUserId == userId).ToListAsync();
            foreach (var Message in Messages)
            {
                Message.Seen = true;
            }
            await _context.SaveChangesAsync(userId, new System.Threading.CancellationToken());
            return await Result.SuccessAsync();
        }

        public async Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> message)
        {
            message.ToUser = await _context.Users.Where(user => user.Id == message.ToUserId).FirstOrDefaultAsync();
            await _context.ChatHistories.AddAsync(_mapper.Map<ChatHistory<Appuser>>(message));
            await _context.SaveChangesAsync();
            return await Result.SuccessAsync();
        }
    }
}