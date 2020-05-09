using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Modulbank.Settings;
using Modulbank.Users.Models;

namespace Modulbank.Users.Services
{
    public interface IJwtTokenService
    {
        UserToken IssueToken(ApplicationUser user);
    }
    
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _jwtOptions;

        public JwtTokenService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
        }

        public  UserToken IssueToken(ApplicationUser user)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secretkey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString().ToLower())
            };

            var utcNow = DateTime.UtcNow;
            
            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                notBefore: utcNow,
                claims: claims,
                expires: utcNow.Add(TimeSpan.FromMinutes(_jwtOptions.TokenLifeTimeInMinutes)),
                signingCredentials: signingCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new UserToken
            {
                Value = encodedJwt,
                UserId = user.Id,
            };

            return response;
        } 
    }
}