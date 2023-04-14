CREATE OR ALTER VIEW SongLookup
AS
  SELECT
  Songs.*,
  (  
    (
      SELECT STRING_AGG(Authors.Nickname, ' ') FROM SongAuthor
      INNER JOIN Authors ON SongAuthor.AuthorsId = Authors.Id
      WHERE SongAuthor.SongsId = Songs.Id
      GROUP BY SongAuthor.SongsId
    ) + Songs.Title + Songs.Album
  ) AS Signature,
  (
    SELECT STRING_AGG(SongGenre.GenresId, ' ') FROM SongGenre
    WHERE SongGenre.SongsId = Songs.Id
    GROUP BY SongGenre.SongsId
  ) AS GenreIds
  FROM Songs
GO