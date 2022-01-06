using Api.Contracts;
using Api.DTO;
using Api.Model;
using Api.ModelOnlineUser;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Service
{
    public class OnlineUserService : IOnlineUserService<ChatUserDto, string>
    {
        private Dictionary<string, OnlineUser> _onlineUsers = new Dictionary<string, OnlineUser>();
        //private List<OnlineUser> _onlineUsers = new List<OnlineUser>();
        IMapper _mapper;
        public OnlineUserService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public bool Add(ChatUserDto userDto)
        {
            var onlineUser = _mapper.Map<ChatUserDto, OnlineUser>(userDto);
            return _onlineUsers.TryAdd(userDto.Id, onlineUser);
        }

        public IEnumerable<ChatUserDto> GetAll()
        {
            return _onlineUsers.Select(e => { return _mapper.Map<OnlineUser, ChatUserDto>(e.Value); });
        }

        public IEnumerable<ChatUserDto> GetAllOther(ChatUserDto user)
        {
            return _onlineUsers
                .Where(e => e.Key != user.Id)
                .Select(e => { return _mapper.Map<OnlineUser, ChatUserDto>(e.Value); });
        }

        public ChatUserDto Get(string id)
        {
            var onlineUser = _onlineUsers.FirstOrDefault(e => e.Key == id).Value;
            if (onlineUser == null)
            {
                return null;
            }
            return _mapper.Map<OnlineUser, ChatUserDto>(onlineUser);
        }

        public ChatUserDto Delete(string connectionId)
        {
            var onlineUser = _onlineUsers.FirstOrDefault(e => e.Value.hubCallerContext.ConnectionId == connectionId).Value;

            if (onlineUser == null ? false : _onlineUsers.Remove(onlineUser.Id))
            {
                return _mapper.Map<OnlineUser, ChatUserDto>(onlineUser);
            }
            return null;
        }
    }
}
