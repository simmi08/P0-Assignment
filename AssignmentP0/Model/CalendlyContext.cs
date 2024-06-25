using Microsoft.EntityFrameworkCore;

namespace AssignmentP0.Model
{
    public class CalendlyContext :DbContext
    {
        public CalendlyContext(DbContextOptions<CalendlyContext> options) : base (options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = "user1",
                    Name = "Saumya Singh",
                    Password = "password123"
                },
                new User
                {
                    UserID = "user2",
                    Name = "Jane Smith",
                    Password = "password456"
                }
            );

            modelBuilder.Entity<Availability>().HasData(
                new Availability
                {
                    ID = 1,
                    UserID = "user1",
                    Date = "2024-06-27",
                    StartTime = DateTime.Parse("2024-06-27T15:00:00"),
                    EndTime = DateTime.Parse("2024-06-27T17:00:00")
                },
                new Availability
                {
                    ID = 2,
                    UserID = "user1",
                    Date = "2024-06-28",
                    StartTime = DateTime.Parse("2024-06-28T15:00:00"),
                    EndTime = DateTime.Parse("2024-06-28T17:00:00")
                },
                new Availability
                {
                    ID = 3,
                    UserID = "user2",
                    Date = "2024-06-27",
                    StartTime = DateTime.Parse("2024-06-27T14:00:00"),
                    EndTime = DateTime.Parse("2024-06-27T18:00:00")
                },
                new Availability
                {
                    ID = 4,
                    UserID = "user2",
                    Date = "2024-06-28",
                    StartTime = DateTime.Parse("2024-06-28T14:00:00"),
                    EndTime = DateTime.Parse("2024-06-28T18:00:00")
                }
            );
        }
    }
}
