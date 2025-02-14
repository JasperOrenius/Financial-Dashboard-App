using Financial_Dashboard_App.Models;
using Financial_Dashboard_App.Services;
using Financial_Dashboard_App.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace Financial_Dashboard_App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationService navigationService;

        public App()
        {
            navigationService = new NavigationService();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            navigationService.CurrentViewModel = new DashboardViewModel();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationService)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
