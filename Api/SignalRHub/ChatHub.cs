using Api.Contracts;
using Api.DTO;
using Api.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Api.SignalRHub
{
    [AllowAnonymous]
    public class ChatHub : Hub
    {

        IMessageService _messageService;
        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Connected");
            
            Clients.Caller.SendAsync("GetUsers",new {users = Clients});
            Clients.Caller.SendAsync("GetUsers", new { user = "someUser" });
            return Task.CompletedTask;
        }


        public async Task Send(MessageView message)
        {
            //var sendMessage = new MessageView() { };
            var messageDTO = new MessageDTO() { 
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
