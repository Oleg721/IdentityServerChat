using Api.DTO;
using Api.Model;
using Api.ModelOnlineUser;
using AutoMapper;


namespace Api.Mapper
{
    public class MapperDAL : Profile
    {
        public MapperDAL()
        {
            MapConfig(this);
        }

        private static void MapConfig(IProfileExpression cfg)
        {
            cfg.CreateMap<ChatUserDto, ChatUser>().ReverseMap();
            cfg.CreateMap<MessageDTO, Message>().ReverseMap();
            cfg.CreateMap<ChatUserDto, OnlineUser>().ReverseMap();
        }
    }
}
