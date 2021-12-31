using Api.DTO;
using Api.DAL;
using Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Contracts;

namespace Api.Repository
{
    public class MessageRepository : IMessageRepository<Message>
    {
        private ChatContext _chatContext;
        public MessageRepository(ChatContext chatContext)
        {
            _chatContext = chatContext;
        }

        public async Task<Message> CreateAsync(Message message)
        {
            var savedMessage = await _chatContext.AddAsync(message);
            _chatContext.SaveChanges();
            if (savedMessage.State == Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                return await _chatContext.FindAsync<Message>(savedMessage.Entity.Id);
            }
            else
            {
                return null;
            }
           

        }


        public Task<IEnumerable<Message>> GetAsync(int count)
        {
            throw new NotImplementedException();
        }
    }
}
