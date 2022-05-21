using Microsoft.EntityFrameworkCore;
using Veille_Technologique.Models;

namespace Veille_Technologique.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Article> Articles { get; set; }
    }
}
