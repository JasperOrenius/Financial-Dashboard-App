using Financial_Dashboard_App.Commands;
using Financial_Dashboard_App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Financial_Dashboard_App.ViewModels
{
    public class NavigationBarViewModel : BaseViewModel
    {
        public ICommand Dashboard { get; }
        public ICommand Transactions { get; }
        public ICommand ImportData { get; }
        public ICommand Reports { get; }

        public NavigationBarViewModel(NavigationService navigationService, Func<DashboardViewModel> createDashboardViewModel, Func<TransactionsViewModel> createTransactionsViewModel, Func<ImportDataViewModel> createImportDataViewModel, Func<ReportsViewModel> createReportsViewModel)
        {
            Dashboard = new NavigateCommand(navigationService, createDashboardViewModel);
            Transactions = new NavigateCommand(navigationService, createTransactionsViewModel);
            ImportData = new NavigateCommand(navigationService, createImportDataViewModel);
            Reports = new NavigateCommand(navigationService, createReportsViewModel);
        }
    }
}
