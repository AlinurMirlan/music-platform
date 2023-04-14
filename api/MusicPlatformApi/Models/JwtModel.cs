using System.ComponentModel.DataAnnotations;

namespace MusicPlatformApi.Models
{
    public class JwtModel
    {
        public string Jwt { get; set; }
        public DateTime Expiration { get; set; }

        public JwtModel(string jwt, DateTime expiration)
        {
            Jwt = jwt;
            Expiration = expiration;
        }
    }
}   
