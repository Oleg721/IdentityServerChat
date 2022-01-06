using Api.Model;
using Microsoft.AspNetCore.SignalR;

namespace Api.DTO
{
    public class ChatUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public UserColors Color { get; set; }
        public HubCallerContext hubCallerContext { get; set; }
    }
}
