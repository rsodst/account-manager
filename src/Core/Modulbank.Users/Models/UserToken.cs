using System;
using System.Text.Json.Serialization;

namespace Modulbank.Users.Models
{
    public class UserToken
    {
        public Guid UserId { get; set; }

        [JsonIgnore]
        public string LoginProvider { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        public string Value { get; set; }
    }
}