using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WalksInNature.Data.Models;

namespace WalksInNature.Data
{
    public class WalksDbContext : IdentityDbContext<User>
    {
        public WalksDbContext(DbContextOptions<WalksDbContext> options)
            : base(options)
        {
        }
        public DbSet<Walk> Walks { get; init; }
        public DbSet<Level> Levels { get; init; }
        public DbSet<Region> Regions { get; init; }
        public DbSet<Event> Events { get; init; }
        public DbSet<Guide> Guides { get; init; }
        public DbSet<Insurance> Insurances { get; init; }
        public DbSet<WalkUser> WalksUsers { get; set; }
        public DbSet<EventUser> EventsUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Walk>()
                .HasOne(x => x.Level)
                .WithMany(x => x.Walks)
                .HasForeignKey(x => x.LevelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Walk>()
                .HasOne(x => x.Region)
                .WithMany(x => x.Walks)
                .HasForeignKey(x => x.RegionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Walk>()
                .HasOne(x => x.AddedByUser)
                .WithMany(x => x.Walks)
                .HasForeignKey(x => x.AddedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
              .Entity<Event>()
              .HasOne(x => x.Level)
              .WithMany(x => x.Events)
              .HasForeignKey(x => x.LevelId)
              .OnDelete(DeleteBehavior.Restrict);

            builder
               .Entity<Event>()
               .HasOne(x => x.Region)
               .WithMany(x => x.Events)
               .HasForeignKey(x => x.RegionId)
               .OnDelete(DeleteBehavior.Restrict);

            builder
              .Entity<Event>()
              .HasOne(x => x.Guide)
              .WithMany(x => x.Events)
              .HasForeignKey(x => x.GuideId)
              .OnDelete(DeleteBehavior.Restrict);

            builder
               .Entity<Guide>()
               .HasOne<User>()
               .WithOne()
               .HasForeignKey<Guide>(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            builder
               .Entity<Insurance>()
               .HasOne(x => x.User)
               .WithMany(x => x.Insurances)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);
            
            builder
                .Entity<WalkUser>()
                .HasKey(x => new { x.WalkId, x.UserId, });           

            builder.Entity<EventUser>()
                .HasKey(e => new { e.EventId, e.UserId, });


            base.OnModelCreating(builder);
        }
    }
}
