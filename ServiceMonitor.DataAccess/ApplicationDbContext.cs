using Microsoft.EntityFrameworkCore;
using ServiceMonitor.DataAccess.Entities;

namespace ServiceMonitor.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<StatusLogEntity> StatusLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb; Database=ServiceMonitorDB; Integrated Security = true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StatusLogEntity>()
                .Property(b => b.StatusLogEntityId)
                .IsRequired();
        }
    }
}

