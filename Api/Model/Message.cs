using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }
}
