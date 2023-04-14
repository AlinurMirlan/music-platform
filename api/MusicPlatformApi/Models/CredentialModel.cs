using System.ComponentModel.DataAnnotations;

namespace MusicPlatformApi.Models
{
    public class CredentialModel
    {
        [EmailAddress]
        public required string Email { get; set; }

        [MinLength(8)]
        public required string Password { get; set; }
    }
}
