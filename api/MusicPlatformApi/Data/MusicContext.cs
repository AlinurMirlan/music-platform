using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicPlatformApi.Data.Entities;

namespace MusicPlatformApi.Data
{
    public class MusicContext : IdentityDbContext<User>
    {
        public DbSet<Song> Songs { get; set; }

        public DbSet<SongLookup> SongLookups { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Playlist> Playlists { get; set; }

        public DbSet<UserSong> UserSong { get; set; }

        public DbSet<PlaylistSong> PlaylistSongs { get; set; }

        public MusicContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Song>()
                .HasMany(song => song.Authors)
                .WithMany(author => author.Songs)
                .UsingEntity(
                    nameof(SongAuthor),
                    l => l.HasOne(typeof(Author))
                        .WithMany()
                        .HasForeignKey(nameof(SongAuthor.AuthorsId))
                        .HasPrincipalKey(nameof(Author.Id)),
                    r => r.HasOne(typeof(Song))
                        .WithMany()
                        .HasForeignKey(nameof(SongAuthor.SongsId))
                        .HasPrincipalKey(nameof(Song.Id)),
                    j => j.HasKey(nameof(SongAuthor.SongsId), nameof(SongAuthor.AuthorsId)));

            builder.Entity<Song>()
                .HasMany(song => song.Genres)
                .WithMany()
                .UsingEntity(
                    nameof(SongGenre),
                    l => l.HasOne(typeof(Genre))
                        .WithMany()
                        .HasForeignKey(nameof(SongGenre.GenresId))
                        .HasPrincipalKey(nameof(Genre.Id)),
                    r => r.HasOne(typeof(Song))
                        .WithMany()
                        .HasForeignKey(nameof(SongGenre.SongsId))
                        .HasPrincipalKey(nameof(Song.Id)),
                    j => j.HasKey(nameof(SongGenre.SongsId), nameof(SongGenre.GenresId)));

            builder.Entity<SongLookup>().ToView("SongLookup");

            builder.Entity<SongLookup>()
                .HasMany(song => song.Authors)
                .WithMany()
                .UsingEntity(
                    nameof(SongAuthor),
                    l => l.HasOne(typeof(Author))
                        .WithMany()
                        .HasForeignKey(nameof(SongAuthor.AuthorsId))
                        .HasPrincipalKey(nameof(Author.Id)),
                    r => r.HasOne(typeof(SongLookup))
                        .WithMany()
                        .HasForeignKey(nameof(SongAuthor.SongsId))
                        .HasPrincipalKey(nameof(SongLookup.Id)),
                    j => j.HasKey(nameof(SongAuthor.SongsId), nameof(SongAuthor.AuthorsId)));

            builder.Entity<SongLookup>()
                .HasMany(song => song.Genres)
                .WithMany()
                .UsingEntity(
                    nameof(SongGenre),
                    l => l.HasOne(typeof(Genre))
                        .WithMany()
                        .HasForeignKey(nameof(SongGenre.GenresId))
                        .HasPrincipalKey(nameof(Genre.Id)),
                    r => r.HasOne(typeof(SongLookup))
                        .WithMany()
                        .HasForeignKey(nameof(SongGenre.SongsId))
                        .HasPrincipalKey(nameof(SongLookup.Id)),
                    j => j.HasKey(nameof(SongGenre.SongsId), nameof(SongGenre.GenresId)));
        }
    }
}
