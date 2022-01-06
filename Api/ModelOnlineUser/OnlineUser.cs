using Api.Model;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ModelOnlineUser
{
    public class OnlineUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public UserColors Color { get; set; }
        public HubCallerContext hubCallerContext { get; set; }

    }
}
