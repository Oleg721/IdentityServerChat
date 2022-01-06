using Api.Contracts;
using Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DAL.Repository
{
    public class UserRepositiry : IUserRepository<ChatUser>
    {
        private ChatContext _chatContext;
        public UserRepositiry(ChatContext chatContext)
        {
            _chatContext = chatContext;
        }
        public Task<bool> AddAsync(ChatUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<ChatUser> GetAsync(string Id)
        {
            return await _chatContext.FindAsync<ChatUser>(Id);
        }

    }
}
