using Financial_Dashboard_App.Services;
using Financial_Dashboard_App.ViewModels;
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
            navigationService.CurrentViewModel = CreateDashBoardViewModel();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationService)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }

        private DashboardViewModel CreateDashBoardViewModel()
        {
            return new DashboardViewModel();
        }

        private TransactionsViewModel CreateTransactionsViewModel()
        {
            return new TransactionsViewModel();
        }

        private ImportDataViewModel CreateImportDataViewModel()
        {
            return new ImportDataViewModel();
        }

        private ReportsViewModel CreateReportsViewModel()
        {
            return new ReportsViewModel();
        }
    }
}
