using Financial_Dashboard_App.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial_Dashboard_App.Services
{
    public static class DatabaseService
    {
        public static void Initialize()
        {
            using(var db = new AppDbContext())
            {
                db.Database.EnsureCreated();
            }
        }
    }
}
