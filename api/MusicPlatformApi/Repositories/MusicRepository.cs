using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MusicPlatformApi.Data;
using MusicPlatformApi.Data.Entities;
using System.Linq.Expressions;

namespace MusicPlatformApi.Repositories
{
    public class MusicRepository : IMusicRepository
    {
        private readonly MusicContext _context;

        public MusicRepository(MusicContext context)
        {
            _context = context;
        }

        public IEnumerable<Song> GetSongs<TKey>(Func<Song, TKey> propertySelector, out int totalPages, bool orderByAscending = true, int page = 1, int items = 6)
        {
            IQueryable<Song> songs = _context.Songs.AsNoTracking()
                            .Include(song => song.Authors)
                            .Include(song => song.Genres);

            IOrderedEnumerable<Song> orderedSongs = orderByAscending ? songs.OrderBy(propertySelector) : songs.OrderByDescending(propertySelector);

            totalPages = (int)Math.Ceiling((double)orderedSongs.Count() / items);
            return orderedSongs
                .Skip((page - 1) * items)
                .Take(items);
        }

        public IEnumerable<Genre> GetGenres()
        {
            return _context.Genres.AsEnumerable();
        }

        public Song? GetSong(int id)
        {
            return _context.Songs.Find(id);
        }

        public IEnumerable<SongLookup> GetLookupSongs<TKey>(Func<SongLookup, TKey> propertySelector, string searchTerm, IEnumerable<int> genreIds, out int totalPages, bool orderByAscending = true, int page = 1, int items = 6)
        {
            string[] tokens = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            IQueryable<SongLookup> songs = _context.SongLookups
                            .Include(song => song.Authors)
                            .Include(song => song.Genres);
            foreach (string token in tokens)
                songs = songs.Where(song => song.Signature.Contains(token));

            foreach (int genreId in genreIds)
                songs = songs.Where(song => song.GenreIds.Contains(genreId.ToString()));

            IOrderedEnumerable<SongLookup> orderedSongs = orderByAscending ? songs.OrderBy(propertySelector) : songs.OrderByDescending(propertySelector);

            totalPages = (int)Math.Ceiling((double)orderedSongs.Count() / items);
            return orderedSongs
                .Skip((page - 1) * items)
                .Take(items);
        }
    }
}
