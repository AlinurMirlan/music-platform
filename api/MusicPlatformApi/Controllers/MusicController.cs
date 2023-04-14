using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPlatformApi.Data.Entities;
using MusicPlatformApi.Models;
using MusicPlatformApi.Repositories;

namespace MusicPlatformApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class MusicController : ControllerBase
    {
        private readonly IMusicRepository _musicRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public MusicController(IMusicRepository musicRepo, IMapper mapper, IConfiguration config)
        {
            _musicRepo = musicRepo;
            _mapper = mapper;
            _config = config;
        }


        [HttpGet("song/{songId}")]
        public IActionResult GetSong([FromRoute]int songId)
        {
            Song? song = _musicRepo.GetSong(songId);
            if (song is null)
                return NotFound();

            string songFolder = _config["Folder:Songs"] ?? throw new InvalidOperationException("Folder for songs is not set.");
            string songPath = Path.Combine(songFolder, song.SongFile);
            if (!System.IO.File.Exists(songPath))
                return NotFound();

            byte[] songRaw = System.IO.File.ReadAllBytes(songPath);
            return File(songRaw, "audio/mpeg");
        }

        [HttpGet("image/{songId}")]
        public IActionResult GetImage([FromRoute]int songId)
        {
            Song? song = _musicRepo.GetSong(songId);
            if (song is null)
                return NotFound();

            string songImageFolder = _config["Folder:SongImages"] ?? throw new InvalidOperationException("Folder for song images is not set.");
            string songImagePath = Path.Combine(songImageFolder, song.ImageFile);
            if (!System.IO.File.Exists(songImagePath))
                return NotFound();

            string fileExtension = songImagePath[^3..^1] == "ng" ? "png" : "jpeg";
            byte[] image = System.IO.File.ReadAllBytes(songImagePath);
            return File(image, $"image/{fileExtension}");
        }

        [HttpGet]
        public ActionResult<IEnumerable<SongDto>> GetSongs(int page = 1, int pageSize = 6, bool orderByAscending = true)
        {
            IEnumerable<Song> songs = _musicRepo.GetSongs((song) => song.ReleaseDate, out int totalPages, orderByAscending, page, pageSize);
            LinkedList<SongDto> songDtos = new();
            foreach (Song song in songs)    
            {
                SongDto songDto = _mapper.Map<SongDto>(song);
                songDtos.AddLast(songDto);
            }

            return Ok(new PageModel<SongDto>()
            {
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                Results = songDtos
            });
        }

        [HttpGet("{searchTerm?}")]
        public ActionResult<IEnumerable<SongDto>> GetSongs([FromQuery]int[] genreIds, [FromRoute]string searchTerm = "", int page = 1, int pageSize = 6, bool orderByAscending = true)
        {
            IEnumerable<SongLookup> songs = _musicRepo.GetLookupSongs((song) => song.ReleaseDate, searchTerm, genreIds, out int totalPages, orderByAscending, page, pageSize);
            LinkedList<SongDto> songDtos = new();
            foreach (SongLookup song in songs)
            {
                SongDto songDto = _mapper.Map<SongDto>(song);
                songDtos.AddLast(songDto);
            }

            return Ok(new PageModel<SongDto>()
            {
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                Results = songDtos
            });
        }

        [HttpGet("genres")]
        public ActionResult<IEnumerable<string>> GetGenres() => Ok(_musicRepo.GetGenres());
    }
}
