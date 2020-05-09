using Microsoft.IdentityModel.Tokens;

namespace Modulbank.Settings
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        
        public string Subject { get; set; }
        public string Audience { get; set; }
        public string Secretkey { get; set; }
        
        public int TokenLifeTimeInMinutes { get; set; }
    }
}