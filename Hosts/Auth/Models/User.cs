using Microsoft.AspNetCore.Identity.MongoDB;

namespace Auth.Models
{
    public class User : IdentityUser
    {
        public User(string userName, string password, string email, RoleType roleType)
        {
            this.UserName = userName;
            this.Password = password;
            this.Email = email;
            this.RoleType = roleType;
        }

        public string Password { get; private set; }

        public RoleType RoleType { get; private set; }
    }
}