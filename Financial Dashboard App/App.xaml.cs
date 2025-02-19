using Financial_Dashboard_App.DbContexts;
using Financial_Dashboard_App.Models;
using Financial_Dashboard_App.Services;
using Financial_Dashboard_App.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;

namespace Financial_Dashboard_App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string connectionString = "Data Source=transactions.db";
        private readonly NavigationService navigationService;
        private readonly IDatabaseService databaseService;

        public App()
        {
            navigationService = new NavigationService();
            databaseService = new DatabaseService(new AppDbContextFactory(connectionString));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(connectionString).Options;
            using(AppDbContext dbContext = new AppDbContext(options))
            {
                dbContext.Database.Migrate();
            }
            navigationService.CurrentViewModel = new DashboardViewModel(databaseService);
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationService, databaseService)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
