using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Modulbank.Shared.Exceptions;
using Modulbank.Users.Commands;
using Modulbank.Users.Messages;
using Modulbank.Users.Models;
using Rebus.Bus;

namespace Modulbank.Users.RequestHandlers
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBus bus;

        public ConfirmEmailCommandHandler(IBus bus, UserManager<ApplicationUser> userManager)
        {
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            var result = await _userManager.ConfirmEmailAsync(user, request.ConfirmationToken);

            if (result.Succeeded == false)
            {
                var errors = result.Errors.Select(p => $"Code:{p.Code}; Description:{p.Description}");
                throw new ApplicationApiException(HttpStatusCode.BadRequest, string.Join("\n", errors));
            }

            await bus.Send(new EmailConfirmedMessage(user.Email));

            return Unit.Value;
        }
    }
}