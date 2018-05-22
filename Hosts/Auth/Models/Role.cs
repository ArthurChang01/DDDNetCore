using System.Collections.Generic;

namespace Auth.Models
{
    public class Role : Microsoft.AspNetCore.Identity.MongoDB.IdentityRole
    {
        public Role(RoleType type, IEnumerable<Authorization> authorization)
        {
            this.Type = type;
            this.Authorization = authorization;
        }

        public RoleType Type { get; private set; }

        public IEnumerable<Authorization> Authorization { get; private set; }
    }
}