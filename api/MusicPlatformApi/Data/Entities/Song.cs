using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlatformApi.Data.Entities
{
    [Index(nameof(Title), nameof(Album))]
    public class Song
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public required string Title { get; set; }

        public List<Author> Authors { get; set; } = new();

        [Column(TypeName = "nvarchar(50)")]
        public required string Album { get; set; }

        public required string ImageFile { get; set; }

        public required string SongFile { get; set; }

        public List<Genre> Genres { get; set; } = new();

        public DateTime ReleaseDate { get; set; }

        public int Popularity { get; set; }
    }
}
