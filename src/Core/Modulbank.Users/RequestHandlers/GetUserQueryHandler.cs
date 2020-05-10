using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Modulbank.Users.Domain;
using Modulbank.Users.Queries;

namespace Modulbank.Users.Query
{
    public class ReadApplicationUserQuery : IRequestHandler<GetUserQuery, ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ReadApplicationUserQuery(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<ApplicationUser> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            return user;
        }
    }
}