/**
 * Author:    Jose Monterroso
 * Partner:   Jasen Lassig, Eli Hebdon
 * Date:      December 6, 2019
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Jose, Eli, Jasen - This work may not be copied for use in Academic Coursework.
 *
 * I, Jose, certify that I wrote this code from scratch and did not copy it in part or whole from 
 * another source.  Any references used in the completion of the assignment are cited in my README file.
 *
 * File Contents
 *
 *    MemeCo Context
 */

using MemeCo.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MemeCo.Models
{
    /// <summary>
    /// MemeCo DB Context
    /// </summary>
    public class MemeCoContext : IdentityDbContext<MemeCoUser>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public MemeCoContext(DbContextOptions<MemeCoContext> options) : base(options) { }

        /// <summary>
        /// Model specified foriegn keys
        /// </summary>
        /// <param name="modelBuilder"></param>
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