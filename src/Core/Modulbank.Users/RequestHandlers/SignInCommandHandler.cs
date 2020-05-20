using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Modulbank.Shared.Exceptions;
using Modulbank.Users.Commands;
using Modulbank.Users.Domain;
using Modulbank.Users.Services;

namespace Modulbank.Users.RequestHandlers
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, UserToken>
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SignInCommandHandler(IJwtTokenService jwtTokenService, UserManager<ApplicationUser> userManager)
        {
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<UserToken> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Email);
            
            if (user == null)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest,"User not found");
            }

            var userIsLockedOut = await _userManager.IsLockedOutAsync(user);

            if (userIsLockedOut)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest,"Account has been deleted");
            }
            
            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (result == false)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest,"Invalid Credential");
            }

            return _jwtTokenService.IssueToken(user);
        }
    }
}