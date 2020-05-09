using System;
using MediatR;

namespace Modulbank.Users.Commands
{
    public interface IConfirmEmailCommandSpecification
    {
        string ConfirmationToken { get; set; }
    }

    public class ConfirmEmailCommand : IRequest, IConfirmEmailCommandSpecification
    {
        public Guid UserId { get; set; } 
        
        public string ConfirmationToken { get; set; }
    }
}