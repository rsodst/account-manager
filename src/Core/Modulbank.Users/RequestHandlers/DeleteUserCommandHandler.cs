using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Modulbank.Shared.Exceptions;
using Modulbank.Users.Commands;
using Modulbank.Users.Domain;

namespace Modulbank.Users.RequestHandlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            
            if (user == null)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest,"User not found");
            }

            user.Email = $"{user.Email}|{DateTime.UtcNow}";

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded == false)
            {
                var errors = String.Join("", result.Errors.Select(p => $"{p.Code}, {p.Description}").ToList());
                throw  new ApplicationApiException(HttpStatusCode.BadRequest, $"Unable to delete account, {errors}");
            }

            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            
            return Unit.Value;
        }
    }
}