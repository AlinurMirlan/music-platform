using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MusicPlatformApi.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlatformApi.Models
{
    public class SongDto
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public List<AuthorDto> Authors { get; set; } = new();

        public required string Album { get; set; }

        public List<Genre> Genres { get; set; } = new();

        public required DateTime ReleaseDate { get; set; }

        public int Popularity { get; set; }
    }
}
