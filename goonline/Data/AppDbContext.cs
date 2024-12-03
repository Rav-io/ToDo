using goonline.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace goonline.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Todo>(entity =>
            {
                entity.HasKey(t => t.id);
                entity.Property(t => t.title).IsRequired().HasMaxLength(100);
                entity.Property(t => t.description).HasMaxLength(500);

                entity.HasData(
                    new Todo
                    {
                        id = 1,
                        title = "Fix critical bug in production",
                        description = "Resolve the issue causing downtime on the payment gateway.",
                        expiryDate = DateTime.Now.AddDays(1),
                        percentComplete = 0,
                        isDone = false
                    },
                    new Todo
                    {
                        id = 2,
                        title = "Complete Project",
                        description = "Finish REST API Task",
                        expiryDate = DateTime.Now.AddDays(3),
                        percentComplete = 50,
                        isDone = false
                    },
                    new Todo
                    {
                        id = 3,
                        title = "Update documentation for REST API",
                        description = "Add detailed API documentation for the new endpoints using Swagger.",
                        expiryDate = DateTime.Now.AddHours(12),
                        percentComplete = 100,
                        isDone = true
                    }
                );
            });
        }
    }
}
