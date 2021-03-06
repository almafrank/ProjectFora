using Microsoft.EntityFrameworkCore;

namespace ProjectFora.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<InterestModel> Interests { get; set; }
        public DbSet<ThreadModel> Threads { get; set; }
        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserInterestModel> UserInterests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data to database
            modelBuilder.Entity<InterestModel>()
                .HasData(new InterestModel() { Id = 1, Name = "Games" },
                new InterestModel() { Id = 2, Name = "Sports" },
                new InterestModel() { Id = 3, Name = "Politics" },
                new InterestModel() { Id = 4, Name = "Religion" },
                new InterestModel() { Id = 5, Name = "Design" },
                new InterestModel() { Id = 6, Name = "Garden" },
                new InterestModel() { Id = 7, Name = "Technology" },
                new InterestModel() { Id = 8, Name = "Pets" });

            // Make Username unique
            modelBuilder.Entity<UserModel>()
                 .HasIndex(b => b.Username)
                 .IsUnique();

            // Many to many (users can have many interests that in turns have many users)
            modelBuilder.Entity<UserInterestModel>()
                .HasKey(ui => new { ui.UserId, ui.InterestId });
            modelBuilder.Entity<UserInterestModel>()
                .HasOne(ui => ui.User)
                .WithMany(u => u.UserInterests)
                .HasForeignKey(ui => ui.UserId);
            modelBuilder.Entity<UserInterestModel>()
                .HasOne(ui => ui.Interest)
                .WithMany(i => i.UserInterests)
                .HasForeignKey(ui => ui.InterestId);

            // Restrict deletion of interest on user delete (set user to null instead)
            modelBuilder.Entity<InterestModel>()
                .HasOne(i => i.User)
                .WithMany(u => u.Interests)
                .HasForeignKey(i => i.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Restrict deletion of thread on user delete (set user to null instead)
            modelBuilder.Entity<ThreadModel>()
                .HasOne(i => i.User)
                .WithMany(u => u.Threads)
                .HasForeignKey(i => i.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Restrict deletion of thread on message delete (set user to null instead)
            modelBuilder.Entity<MessageModel>()
                .HasOne(i => i.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(i => i.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }

        
    }
}
