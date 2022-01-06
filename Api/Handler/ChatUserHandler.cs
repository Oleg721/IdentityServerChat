using Api.Contracts;
using Api.DTO;
using Api.ModelView;
using Api.Service;
using Api.SignalRHub;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Handler
{
    public interface MyHubContext<THub> : IHubContext<THub> where THub:Hub 
    {
        THub Hub { get; set; }
    }
    public class ChatUserHandler
    {
        private IMessageService _messageService;
        private IUserService<ChatUserDto, string> _userService;
        private OnlineUserService _onlineUserService;
        private IMapper _mapper;
        private IHubContext<ChatHub> _hubContext;

        public ChatUserHandler(
            IUserService<ChatUserDto, string> userService,
            IMessageService messageService,
            OnlineUserService onlineUserService,
            IHubContext<ChatHub> hubContext,
            IMapper mapper)
        {
            _onlineUserService = onlineUserService;
            _messageService = messageService;
            _onlineUserService = onlineUserService;
            _userService = userService;
            _mapper = mapper;
            _hubContext = hubContext;
        }


        public async Task AddUserAsync(string userId, HubCallerContext hubCallerContext)
        {
            if (userId == null)
            {
                await _hubContext.Clients.Clients(hubCallerContext.ConnectionId)
                    .SendAsync("Error", new { msg = "User id not found" });
                hubCallerContext.Abort();
                return;
            }

            var userDto = await _userService.GetAsync(userId);
            if (userDto == null)
            {
                await _hubContext.Clients.Clients(hubCallerContext.ConnectionId)
                    .SendAsync("Error", new { msg = "User not found" });
                hubCallerContext.Abort();
                return;
            };

            var connectedUser = _onlineUserService.Get(userId);
            if (connectedUser != null)
            {
                connectedUser.hubCallerContext.Abort();
                _onlineUserService.Delete(connectedUser.hubCallerContext.ConnectionId);
            }

            userDto.hubCallerContext = hubCallerContext;
            _onlineUserService.Add(userDto);

            var onlineUsers = _onlineUserService.GetAll()
                .Select(e => _mapper.Map<ChatUserDto, UserView>(e));
            var userView = _mapper.Map<ChatUserDto, UserView>(userDto);

            await _hubContext.Clients.Clients(hubCallerContext.ConnectionId)
                .SendAsync("GetUsers", onlineUsers);
            await _hubContext.Clients.AllExcept(new List<string>{hubCallerContext.ConnectionId})
                .SendAsync("User", "add", userView);
        }

        public async Task<bool> Delete(string userId)
        {
            var deletedUser = _onlineUserService.Delete(userId);
            if (deletedUser != null)
            {
                await _hubContext.Clients.All
                    .SendAsync("User", "delete", deletedUser);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
