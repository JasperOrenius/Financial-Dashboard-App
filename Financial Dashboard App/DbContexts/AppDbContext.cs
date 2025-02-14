using Financial_Dashboard_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Financial_Dashboard_App.DbContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=transactions.db");
        }
    }
}
