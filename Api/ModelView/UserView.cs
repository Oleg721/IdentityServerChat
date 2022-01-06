using Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ModelView
{
    public class UserView
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public UserColors Color { get; set; }

    }
}
