using Microsoft.EntityFrameworkCore;
using SampleStoreWebApp.Models;

namespace SampleStoreWebApp.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
