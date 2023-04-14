using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace MusicPlatformApi.Data.Entities
{
    public class SongLookup
    {
        [Key]
        public int Id { get; set; }

        public required string Title { get; set; }

        public List<Author> Authors { get; set; } = new();

        public required string Album { get; set; }

        public required string ImageFile { get; set; }

        public required string SongFile { get; set; }

        public List<Genre> Genres { get; set; } = new();

        public DateTime ReleaseDate { get; set; }

        public int Popularity { get; set; }

        public required string Signature { get; set; }
        
        public required string GenreIds { get; set; }
    }
}
