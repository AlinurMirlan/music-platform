using MusicPlatformApi.Data.Entities;
using MusicPlatformApi.Models;

namespace MusicPlatformApi.Repositories
{
    public interface IJwtTokenRepository
    {
        public JwtModel CreateJwt(User user);
    }
}
