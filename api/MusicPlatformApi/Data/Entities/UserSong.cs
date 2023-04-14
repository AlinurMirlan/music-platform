using Microsoft.EntityFrameworkCore;

namespace MusicPlatformApi.Data.Entities
{
    [PrimaryKey(nameof(SongId), nameof(UserId))]
    public class UserSong
    {
        public int SongId { get; set; }

        public required Song Song { get; set; }

        public required string UserId { get; set; }

        public required User User { get; set; }

        public DateTime DateTimeAdded { get; set; }
    }
}
