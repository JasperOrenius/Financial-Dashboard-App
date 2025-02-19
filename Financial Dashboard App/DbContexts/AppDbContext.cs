using Financial_Dashboard_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Financial_Dashboard_App.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
