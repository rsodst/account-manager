using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Modulbank.Users.Models
{
    public class ApplicationUser 
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string UserName { get; set; }

        [JsonIgnore]
        public string NormalizedUserName { get; set; }

        public string Email { get; set; }

        [JsonIgnore]
        public string NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        [JsonIgnore]
        public string SecurityStamp { get; set; }

        [JsonIgnore]
        public string ConcurrencyStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        internal List<Claim> Claims { get; set; }

        internal List<UserRole> Roles { get; set; }

        internal List<UserLoginInfo> Logins { get; set; }

        internal List<UserToken> Tokens { get; set; }
    }
}