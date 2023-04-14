using CsvHelper;
using CsvHelper.TypeConversion;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MusicPlatformApi.Data.Entities;
using MusicPlatformApi.Infrastructure;
using System.Globalization;

namespace MusicPlatformApi.Data
{
    public class MusicDbInitializer
    {
        private readonly MusicContext _context;
        private readonly ILogger<MusicDbInitializer> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _environment;

        public MusicDbInitializer(MusicContext context, ILogger<MusicDbInitializer> logger, UserManager<User> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _environment = environment;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            if (!_userManager.Users.Any())
            {
                User user = new()
                {
                    Name = "Alinur",
                    Email = "alinur@gmail.com",
                    UserName = "alinur@gmail.com",
                    Age = 20,
                    Sex = "m"
                };

                IdentityResult result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                    throw new InvalidOperationException("Failed to create user while seeding database.");

                result = await _userManager.AddPasswordAsync(user, "P@ssw0rd?");
                if (!result.Succeeded)
                    throw new InvalidOperationException("Failed to set password while seeding database.");
            }

            if (!_context.Songs.Any())
            {
                _logger.LogInformation("Calling seeding of the database");
                string csvFilePath = @$"{_environment.ContentRootPath}/Data/songs.csv";
                SongReader songReader = new(csvFilePath: csvFilePath);
                Song[] songs = songReader.ReadSongs();
                _context.AddRange(songs);
                await _context.SaveChangesAsync();
            }
        }
    }
}
