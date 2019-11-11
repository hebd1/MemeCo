using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemeCo.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MemeCo.Models
{
    public class MemeCoContext : IdentityDbContext<MemeCoUser>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public MemeCoContext(DbContextOptions<MemeCoContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            modelBuilder.Entity<Follow>().HasKey(x => new { x.UserID, x.FollowerID });

            modelBuilder.Entity<Follow>()
                .HasOne(x => x.User)
                .WithMany(f => f.Followers)
                .HasForeignKey(x => x.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Follow>()
                .HasOne(x => x.Follower)
                .WithMany(f => f.Following)
                .HasForeignKey(x => x.FollowerID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
                .HasMany(x => x.Comments)
                .WithOne(y => y.Post);
        }
    }
}
