using System;

namespace Modulbank.Users.Domain
{
    internal class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
