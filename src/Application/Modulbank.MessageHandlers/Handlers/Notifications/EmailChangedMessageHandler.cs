using System;
using System.Threading.Tasks;
using Modulbank.Users.Messages;
using Rebus.Handlers;

namespace Modulbank.MessageHandlers.Handlers.Notifications
{
    public class EmailChangedMessageHandler : IHandleMessages<EmailConfirmedMessage>
    {
        public Task Handle(EmailConfirmedMessage message)
        {
            // TODO: Notify user
            
            throw new NotImplementedException();
        }
    }
}