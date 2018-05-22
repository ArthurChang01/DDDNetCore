using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace Auth.Models
{
    public class AuthContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
        private readonly IConfiguration _config;

        #region Properties

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }

        #endregion Properties

        #region Constructor

        public AuthContext()
        //: this(new DbContextOptions<AuthContext>())
        {
        }

        public AuthContext(DbContextOptions<AuthContext> authDbCtxOpt, IConfiguration config)
            : base(authDbCtxOpt)
        {
            this._config = config;
        }

        #endregion Constructor

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
        }
    }
}