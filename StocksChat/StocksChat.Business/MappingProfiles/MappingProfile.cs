using AutoMapper;
using StocksChat.Business.Models;
using StocksChat.Persistence.Entities;

namespace StocksChat.Business.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUserEntity, AppUser>().ReverseMap();
            CreateMap<MessageEntity, Message>().ReverseMap();
        }
    }
}