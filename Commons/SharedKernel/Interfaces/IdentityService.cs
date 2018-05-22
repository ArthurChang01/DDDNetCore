using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SharedKernel.Interfaces
{
    public interface IIdentityService
    {
        ClaimsPrincipal GetUserIdentity();
    }
}