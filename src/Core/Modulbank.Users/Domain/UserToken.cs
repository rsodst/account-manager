using System;
using System.Text.Json.Serialization;

namespace Modulbank.Users.Domain
{
    public class UserToken
    {
        public Guid UserId { get; set; }

        public string Email { get; set; }
        
        public DateTime IssuedDate { get; set; }
        
        [JsonIgnore]
        public string LoginProvider { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        public string Value { get; set; }
    }
}