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
using Modulbank.Users.Services;

namespace Modulbank.Users.RequestHandlers
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, UserToken>
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SignUpCommandHandler(IJwtTokenService jwtTokenService, UserManager<ApplicationUser> userManager)
        {
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<UserToken> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded == false)
            {
                var errors = result.Errors.Select(p => $"Code:{p.Code}; Description:{p.Description}");
                throw new ApplicationApiException(HttpStatusCode.BadRequest,string.Join("\n", errors));
            }

            return _jwtTokenService.IssueToken(user);
        }
    }
}