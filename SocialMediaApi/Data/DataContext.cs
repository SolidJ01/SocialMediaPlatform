using Microsoft.EntityFrameworkCore;

namespace SocialMediaApi.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<LoginToken> LoginTokens { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasMany(p => p.LikedBy)
                .WithMany(p => p.LikedPosts);
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Poster)
                .WithMany(p => p.Posts);
        }
    }
}
