using Api.DTO;
using Api.Model;
using Api.ModelView;
using AutoMapper;


namespace Api.Mapper
{
    public class MapperView : Profile
    {
        public MapperView()
        {
            MapConfig(this);
        }

        private static void MapConfig(IProfileExpression cfg)
        {
            cfg.CreateMap<ChatUserDto, UserView>().ReverseMap();
            cfg.CreateMap<MessageDTO, MessageView>().ReverseMap();
        }
    }
}
