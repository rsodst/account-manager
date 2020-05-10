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
    public class UpdatePasswordHandlerHandler : IRequestHandler<UpdatePasswordCommand>
    {
        private UserManager<ApplicationUser> _userManager;

        public UpdatePasswordHandlerHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest, "User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            
            if (result.Succeeded == false)
            {
                var errors = result.Errors.Select(p => $"Code:{p.Code}; Description:{p.Description}");
                throw new ApplicationApiException(HttpStatusCode.BadRequest,string.Join("\n", errors));
            }

            return Unit.Value;
        }
    }
}