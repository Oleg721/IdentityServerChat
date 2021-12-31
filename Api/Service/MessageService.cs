using Api.Contracts;
using Api.DTO;
using Api.Model;
using System;
using System.Collections.Generic;
using Api.Contracts;
using System.Threading.Tasks;

namespace Api.Service
{
    public class MessageService : IMessageService
    {
        private IMessageRepository<Message> _messageRepository;
        public MessageService(IMessageRepository<Message> messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task<MessageDTO> CreateMessage(MessageDTO message)
        {
            var newMessage = new Message
            {
                UserId = message.UserId,
                Date = message.Date,
                Text = message.Text
            };
            var savedMessage = await _messageRepository.CreateAsync(newMessage);
            if (savedMessage == null)
            {
                return null;
            }
            return new MessageDTO() { Id = savedMessage.Id, 
                Date = savedMessage.Date, 
                Text = savedMessage.Text, 
                UserId = savedMessage.UserId
            };
        }

        public Task<IEnumerable<MessageDTO>> getMessagesAsync(int count)
        {
            throw new NotImplementedException();
        }
    }
}
