using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Modulbank.App
{
    public class AppController : ControllerBase
    {
        public Guid CurrentUserId => Guid.Parse(HttpContext.User?.Claims?.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new Exception($"UserId is null or empty"));
    }
}