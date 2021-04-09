using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using SocialMedia.Infrastructure;
using SocialMedia.Models;

namespace SocialMedia.Data
{
    public class SocialMediaDbContext :
        IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {

        public SocialMediaDbContext(DbContextOptions<SocialMediaDbContext> options)
              : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Board> Boards { get; set; }
        public DbSet<Users> UserList { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Column> Columns { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
