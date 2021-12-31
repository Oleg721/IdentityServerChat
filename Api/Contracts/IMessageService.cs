using Api.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Contracts
{
    public interface IMessageService
    {
        public Task<IEnumerable<MessageDTO>> getMessagesAsync(int count);

        public Task<MessageDTO> CreateMessage(MessageDTO message);
    }
}
