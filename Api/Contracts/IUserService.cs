using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Contracts
{
   public interface IUserService<TDto, TId>
    {
        public Task<TDto> GetAsync(TId id);
    }
}
