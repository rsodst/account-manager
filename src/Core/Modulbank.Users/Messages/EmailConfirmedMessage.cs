using System;

namespace Modulbank.Users.Messages
{
    public class EmailConfirmedMessage
    {
        public string Email { get; set; }
        
        public EmailConfirmedMessage(string email)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }
    }
}