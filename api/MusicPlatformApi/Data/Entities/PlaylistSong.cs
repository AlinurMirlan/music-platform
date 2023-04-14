using Microsoft.EntityFrameworkCore;

namespace MusicPlatformApi.Data.Entities
{
    [PrimaryKey(nameof(PlaylistId), nameof(SongId))]
    public class PlaylistSong
    {
        public int PlaylistId { get; set; }

        public required Playlist Playlist { get; set; }

        public int SongId { get; set; }

        public required Song Song { get; set; }

        public DateTime DateTimeAdded { get; set; }
    }
}
