using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Api.Model
{
    public class ChatUser
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public UserColors Color { get; set; }
        public List<Message> messages { get; set; } = new List<Message>();
    }
}
