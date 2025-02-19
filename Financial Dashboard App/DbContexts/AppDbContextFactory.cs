using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial_Dashboard_App.DbContexts
{
    public class AppDbContextFactory
    {
        private readonly string connectionString;

        public AppDbContextFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public AppDbContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(connectionString).Options;
            return new AppDbContext(options);
        }
    }
}
