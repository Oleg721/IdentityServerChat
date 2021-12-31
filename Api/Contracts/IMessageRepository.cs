using Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Contracts
{
    public interface IMessageRepository<T> where T : Message
    {
        public Task<IEnumerable<T>> GetAsync(int count);
        public Task<T> CreateAsync(T message);

    }
}
