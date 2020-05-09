using System;

namespace Modulbank.Users.Messages
{
    public class EmailUpdatedMessage
    {
        public string Email { get; set; }
        
        public string ConfirmationToken { get; set; }

        public EmailUpdatedMessage(string email, string confirmationToken)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            ConfirmationToken = confirmationToken ?? throw new ArgumentNullException(nameof(confirmationToken));
        }
    }
}