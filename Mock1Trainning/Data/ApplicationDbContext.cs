using Microsoft.EntityFrameworkCore;
using Mock1Trainning.Models;

namespace Mock1Trainning.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
            base(options)
        { }
        public DbSet<Villa> Villas { get; set; }

    }
}
