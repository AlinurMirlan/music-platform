using MusicPlatformApi.Data.Entities;

namespace MusicPlatformApi.Repositories
{
    public interface IMusicRepository
    {
        public Song? GetSong(int id);
        public IEnumerable<Genre> GetGenres();

        public IEnumerable<Song> GetSongs<TKey>(Func<Song, TKey> propertySelector, out int totalPages, bool orderByAscending = true, int page = 1, int items = 6);

        public IEnumerable<SongLookup> GetLookupSongs<TKey>(Func<SongLookup, TKey> propertySelector, string searchTerm, IEnumerable<int> genreIds, out int totalPages, bool orderByAscending = true, int page = 1, int items = 6);
    }
}
