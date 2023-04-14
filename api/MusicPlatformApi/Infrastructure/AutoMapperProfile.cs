using AutoMapper;
using MusicPlatformApi.Data.Entities;
using MusicPlatformApi.Models;

namespace MusicPlatformApi.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            base.CreateMap<Author, AuthorDto>();
            base.CreateMap<Song, SongDto>()
                .AfterMap((song, songDto, context) => songDto.Authors = context.Mapper.Map<List<AuthorDto>>(song.Authors));
            base.CreateMap<SongLookup, SongDto>();
            base.CreateMap<UserModel, User>()
                .ForMember(user => user.UserName, options => options.MapFrom(userModel => userModel.Email));
        }
    }
}
