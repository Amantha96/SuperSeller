using Microsoft.EntityFrameworkCore;
using SuperSeller.Models;

namespace SuperSeller.Services
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }

    }
}
