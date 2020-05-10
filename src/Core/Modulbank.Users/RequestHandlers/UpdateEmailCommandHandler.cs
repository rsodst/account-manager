using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Modulbank.Shared.Exceptions;
using Modulbank.Users.Commands;
using Modulbank.Users.Domain;
using Modulbank.Users.Messages;
using Rebus.Bus;

namespace Modulbank.Users.RequestHandlers
{
    public class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBus bus;
        private readonly ILogger<UpdateEmailCommandHandler> logger;

        public UpdateEmailCommandHandler(IBus bus, ILogger<UpdateEmailCommandHandler> logger, UserManager<ApplicationUser> userManager)
        {
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<Unit> Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null) throw new ApplicationApiException(HttpStatusCode.BadRequest, "User not found");

            user.Email = request.Email;
            user.UserName = request.Email;

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded == false)
            {
                var errors = updateResult.Errors.Select(p => $"Code:{p.Code}; Description:{p.Description}");
                throw new ApplicationApiException(HttpStatusCode.BadRequest, string.Join("\n", errors));
            }

            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            logger.LogInformation($"Email confirmation token:{confirmationToken}");

            await bus.Send(new EmailUpdatedMessage(user.Email, confirmationToken));

            return Unit.Value;
        }
    }
}