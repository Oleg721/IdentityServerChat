using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Contracts
{
    public interface IOnlineUserService<TDto, TId> where TDto : class
    {
        public bool Add(TDto userDto);
        public IEnumerable<TDto> GetAll();
        public TDto Get(TId id);
        public IEnumerable<TDto> GetAllOther(TDto user);
    }
}
