using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext( DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<UserLike> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLike>()
            .HasKey(s => new {s.LikedUserId,s.SourceUserId});

            builder.Entity<UserLike>()
            .HasOne(s=>s.SourceUser)
            .WithMany(s =>s.LikedUsers)
            .HasForeignKey(s=>s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

             builder.Entity<UserLike>()
            .HasOne(s=>s.LikedUser)
            .WithMany(s =>s.LikedByUsers)
            .HasForeignKey(s=>s.LikedUserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
            .HasOne(u=>u.Recipient)
            .WithMany(m=>m.MessagesRecieved)
            .OnDelete(DeleteBehavior.Restrict);

              builder.Entity<Message>()
            .HasOne(u=>u.Sender)
            .WithMany(m=>m.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}