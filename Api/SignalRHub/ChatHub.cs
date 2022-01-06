using Api.Contracts;
using Api.DTO;
using Api.Handler;
using Api.ModelView;
using Api.Service;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;
using Api.Util;
using System.Threading.Tasks;

namespace Api.SignalRHub
{


    //[AllowAnonymous]
    public class ChatHub : Hub
    {
        private ChatUserHandler _chatUserHandler;
        private IMessageService _messageService;
        private IUserService<ChatUserDto, string> _userService;
        public ChatHub(
            IUserService<ChatUserDto, string> userService,
            IMessageService messageService,
            OnlineUserService onlineUserService,
            ChatUserHandler chatUserHandler,
            IMapper mapper)
        {
            _messageService = messageService;
            _userService = userService;
            _chatUserHandler = chatUserHandler;
        }


        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("Connected");
            await _chatUserHandler.AddUserAsync(Context.User.GetId(), Context);
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"Disconnect user {Context.ConnectionId}");
           // await Clients.All.SendAsync("User","delete", "{}");
            // _chatUserHandler.Delete(Context.User.GetId());
            await _chatUserHandler.Delete(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }


        public async Task Send(MessageView message)
        {
            //var sendMessage = new MessageView() { };
            var messageDTO = new MessageDTO()
            {
                UserId = message.UserId,
                Date = message.Date,
                Text = message.Text
            };
            var savedMessage = await _messageService.CreateMessage(messageDTO);
            if (savedMessage == null)
            {
                await this.Clients.Caller.SendAsync("Error", new { data = "message not saved" });
            }
            await this.Clients.All.SendAsync("Send", message);
        }
    }
}
