using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class BlogDbContext : IdentityDbContext<BlogUser>
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }
        
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PostTag>(x => x.HasKey(pt => new { pt.PostId, pt.TagId }));

            builder.Entity<PostTag>()
            .HasOne(u => u.Post)
            .WithMany(u => u.PostTags)
            .HasForeignKey(u => u.PostId);

            builder.Entity<PostTag>()
            .HasOne(u => u.Tag)
            .WithMany(u => u.PostTags)
            .HasForeignKey(u => u.TagId);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}