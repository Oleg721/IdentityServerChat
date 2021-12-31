using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Contracts
{
    interface IUserRepository<T>
    {
        public bool Add(T user);
        public IEnumerable<T> GetAll();
        public IEnumerable<T> GetAllOther(T user);
    }
}
