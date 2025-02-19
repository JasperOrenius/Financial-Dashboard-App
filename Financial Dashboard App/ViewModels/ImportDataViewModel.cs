using Financial_Dashboard_App.Commands;
using Financial_Dashboard_App.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ClosedXML.Excel;
using Financial_Dashboard_App.Services;

namespace Financial_Dashboard_App.ViewModels
{
    public class ImportDataViewModel : BaseViewModel
    {
        private readonly IDatabaseService databaseService;

        public ObservableCollection<Transaction> Transactions { get; set; } = new ObservableCollection<Transaction>();

        public List<string> TransactionTypes { get; } = new List<string>
        {
            "Income", "Expense"
        };

        private string selectedTransactionType;
        public string SelectedTransactionType
        {
            get => selectedTransactionType;
            set
            {
                selectedTransactionType = value;
                OnPropertyChanged(nameof(SelectedTransactionType));
            }
        }

        private decimal amount;
        public decimal Amount
        {
            get => amount;
            set
            {
                amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        private string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public DateTime TransactionDate { get; set; } = DateTime.Today;
        public string SelectedFileName { get; set; }

        public ICommand AddEntryCommand { get; }
        public ICommand BrowseFilesCommand { get; }
        public ICommand ImportExcelCommand { get; }

        public ImportDataViewModel(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
            AddEntryCommand = new RelayCommand(AddEntry);
            BrowseFilesCommand = new RelayCommand(BrowseFiles);
        }

        private async Task AddEntry()
        {
            if(string.IsNullOrEmpty(SelectedTransactionType) || Amount <=0 || string.IsNullOrEmpty(Description))
            {
                return;
            }
            var newTransaction = new Transaction
            {
                Type = SelectedTransactionType,
                Description = Description,
                Amount = Amount,
                Date = TransactionDate
            };
            await databaseService.CreateTransaction(newTransaction);
            Transactions.Add(newTransaction);
            SelectedTransactionType = string.Empty;
            Description = string.Empty;
            Amount = 0;
            OnPropertyChanged(nameof(SelectedTransactionType));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(Amount));
            MessageBox.Show("Transaction added");
        }

        private async Task BrowseFiles()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx*",
                Title = "Select an Excel file"
            };

            if(openFileDialog.ShowDialog() == true)
            {
                SelectedFileName = openFileDialog.FileName;
                OnPropertyChanged(nameof(SelectedFileName));
            }
        }

        private async Task ImportExcel()
        {
            if(string.IsNullOrEmpty(SelectedFileName) || !File.Exists(SelectedFileName))
            {
                return;
            }
            try
            {
                var importedTransactions = new List<Transaction>();
                await Task.Run(() =>
                {
                    using(var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RangeUsed().RowsUsed();
                        foreach(var row in rows.Skip(1))
                        {
                            var transaction = new Transaction
                            {
                                Date = DateTime.Parse(row.Cell(1).GetString()),
                                Description = row.Cell(2).GetString(),
                                Amount = decimal.Parse(row.Cell(3).GetString()),
                                Type = row.Cell(4).GetString()
                            };
                            importedTransactions.Add(transaction);
                        }
                    }
                });
                foreach(var transaction in importedTransactions)
                {
                    await databaseService.CreateTransaction(transaction);
                    Transactions.Add(transaction);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
