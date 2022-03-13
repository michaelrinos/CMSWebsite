using Microsoft.EntityFrameworkCore;

namespace CMSWebsite.Models
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
        public DbSet<RazerView> Views { get; set; }
    }
}
