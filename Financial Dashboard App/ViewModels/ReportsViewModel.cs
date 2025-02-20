using Financial_Dashboard_App.Commands;
using Financial_Dashboard_App.Models;
using Financial_Dashboard_App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Financial_Dashboard_App.ViewModels
{
    public class ReportsViewModel : BaseViewModel
    {
        private readonly IDatabaseService databaseService;

        public ObservableCollection<Transaction> Transactions { get; set; } = new ObservableCollection<Transaction>();

        private DateTime startDate = DateTime.Today.AddMonths(-1);
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime endDate = DateTime.Today;
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value; 
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public ICommand GenerateReportCommand { get; }
        public ICommand ExportToPDFCommand { get; }
        public ICommand ExportToExcelCommand { get; }

        public ReportsViewModel(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
            GenerateReportCommand = new RelayCommand(GenerateReport);
        }

        private async Task GenerateReport()
        {
            Transactions.Clear();
            var allTransactions = await databaseService.GetAllTransactions();
            var filteredTransactions = allTransactions.Where(t => t.Date >= StartDate && t.Date <= EndDate).OrderBy(t => t.Date);
            foreach(var transaction in filteredTransactions)
            {
                Transactions.Add(transaction);
            }
        }
    }
}
