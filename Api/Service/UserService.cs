using Api.Contracts;
using Api.DAL.Repository;
using Api.DTO;
using Api.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Service
{
    public class UserService : IUserService<ChatUserDto, string>
    {
        private IUserRepository<ChatUser> _userRepositiry;
        IMapper _mapper;
        public UserService(IUserRepository<ChatUser> userRepositiry, IMapper mapper)
        {
            _userRepositiry = userRepositiry;
            _mapper = mapper;
        }
        public async Task<ChatUserDto> GetAsync(string id)
        {
            var user = await _userRepositiry.GetAsync(id);
            var userDto = _mapper.Map<ChatUserDto>(user);
            return userDto;
        }

    }
}
