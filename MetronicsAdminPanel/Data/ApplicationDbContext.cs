using MetronicsAdminPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace MetronicsAdminPanel.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)

        {
            
        }
        public DbSet<Sliders> Sliders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify SliderId as the primary key
            modelBuilder.Entity<Sliders>().HasKey(s => s.SliderId);
        }
    }
}
