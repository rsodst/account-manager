using System;
using System.Threading.Tasks;
using Modulbank.Users.Messages;
using Rebus.Handlers;

namespace Modulbank.MessageHandlers.Handlers.Notifications
{
    public class EmailUpdatedMessageHandler : IHandleMessages<EmailUpdatedMessage>
    {
        public Task Handle(EmailUpdatedMessage message)
        {
            // TODO: Send confirmation email
            
            throw new NotImplementedException();
        }
    }
}