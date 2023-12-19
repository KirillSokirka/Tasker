using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tasker.Application.Interfaces.Queries;
using Tasker.Domain.Entities.Identity;

namespace Tasker.Application.Queries
{
    public class GetUserQuery : IGetUserQuery
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserQuery(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string?> GetUserId(HttpContext context)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                return null;
            }

            var user = await _userManager.FindByEmailAsync(userIdClaim.Value);

            return user?.Id;
        }
    }
}
