using System;

namespace Api.DTO
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
    }
}
