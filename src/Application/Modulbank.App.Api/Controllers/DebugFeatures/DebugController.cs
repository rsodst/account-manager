using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modulbank.Profiles;
using Modulbank.Profiles.Domain;
using Modulbank.Users.Tables;
using Swashbuckle.AspNetCore.Annotations;

namespace Modulbank.App.Api.Controllers.Debug
{
    [Route("debug/profile-confirmation")]
    [ApiController, Authorize]
    public class DebugController : AppController
    {
        private readonly IProfilesContext _profilesContext;

        public DebugController(IProfilesContext profilesContext)
        {
            _profilesContext = profilesContext ?? throw new ArgumentNullException(nameof(profilesContext));
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] {"Debug"})]
        public async Task<IActionResult> ConfirmUserProfile()
        {
            var table = new ProfileConfirmationTable(_profilesContext);

            var result = await table.CreateAsync(new ProfileConfirmation
            {
                UserId = CurrentUserId,
                Description = "Test confirmation",
                IsDeleted = false,
                LastModified = DateTime.UtcNow,
                CreationDate = DateTime.UtcNow,
            });

            return Ok(result);
        }
        
        [HttpPut]
        [SwaggerOperation(Tags = new[] {"Debug"})]
        public async Task<IActionResult> UpdateConfirmation(bool isDeleted)
        {
            var table = new ProfileConfirmationTable(_profilesContext);

            var confirmation = await table.GetAsync(CurrentUserId);
            
            if (confirmation == null)
            {
                return BadRequest("Confirmation not found");
            }

            confirmation.IsDeleted = isDeleted;
            confirmation.LastModified = DateTime.UtcNow;

            await table.UpdateAsync(confirmation);
            
            return Ok(confirmation);
        }
    }
}