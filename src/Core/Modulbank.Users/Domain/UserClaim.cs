using System;

namespace Modulbank.Users.Domain
{
    internal class UserClaim
    {
        public UserClaim()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
