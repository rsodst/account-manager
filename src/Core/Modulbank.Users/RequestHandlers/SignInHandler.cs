using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Modulbank.Shared.Exceptions;
using Modulbank.Users.Commands;
using Modulbank.Users.Models;
using Modulbank.Users.Services;

namespace Modulbank.Users.RequestHandlers
{
    public class SignInHandler : IRequestHandler<SignInCommand, UserToken>
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SignInHandler(IJwtTokenService jwtTokenService, UserManager<ApplicationUser> userManager)
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

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (result == false)
            {
                throw new ApplicationApiException(HttpStatusCode.BadRequest,"Invalid Credential");
            }

            return _jwtTokenService.IssueToken(user);
        }
    }
}