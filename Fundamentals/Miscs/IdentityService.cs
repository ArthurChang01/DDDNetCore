using Microsoft.AspNetCore.Http;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Fundamentals.Miscs
{
    public class IdentityService : IIdentityService
    {
        private IHttpContextAccessor _context;

        public IdentityService(IHttpContextAccessor context)
        {
            this._context = context ?? throw new ArgumentException(nameof(context));
        }

        public ClaimsPrincipal GetUserIdentity()
        {
            return this._context.HttpContext.User;
        }
    }
}