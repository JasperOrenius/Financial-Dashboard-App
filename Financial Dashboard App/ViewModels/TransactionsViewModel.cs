using Financial_Dashboard_App.Commands;
using Financial_Dashboard_App.Models;
using Financial_Dashboard_App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Financial_Dashboard_App.ViewModels
{
    public class TransactionsViewModel : BaseViewModel
    {
        private readonly IDatabaseService databaseService;

        public ObservableCollection<Transaction> Transactions { get; set; } = new ObservableCollection<Transaction>();
        public ObservableCollection<Transaction> FilteredTransactions { get; set; } = new ObservableCollection<Transaction>();
        public List<string> TransactionTypes { get; } = new List<string> { "All", "Income", "Expense" };

        private Transaction selectedTransaction;
        public Transaction SelectedTransaction
        {
            get => selectedTransaction;
            set
            {
                selectedTransaction = value;
                OnPropertyChanged(nameof(SelectedTransaction));
            }
        }

        private string selectedFilter = "All";
        public string SelectedFilter
        {
            get => selectedFilter;
            set
            {
                selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                FilterTransactions();
            }
        }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterTransactions();
            }
        }

        private Transaction? editingTransaction;
        public Transaction? EditingTransaction
        {
            get => editingTransaction;
            set
            {
                editingTransaction = value;
                OnPropertyChanged(nameof(EditingTransaction));
            }
        }

        public bool IsEditing => EditingTransaction != null;

        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public TransactionsViewModel(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
            EditCommand = new EditTransactionCommand(EditTransaction);
            SaveCommand = new SaveTransactionCommand(SaveTransaction);
            DeleteCommand = new DeleteTransactionCommand(DeleteTransaction);
            LoadTransactions();
        }

        private async Task LoadTransactions()
        {
            var transactions = await databaseService.GetAllTransactions();
            Transactions.Clear();
            foreach (var transaction in transactions)
            {
                Transactions.Add(transaction);
            }
            FilterTransactions();
        }

        private void FilterTransactions()
        {
            var filteredTransactions = Transactions.Where(t =>
                (SelectedFilter == "All" || t.Type == selectedFilter) &&
                (string.IsNullOrWhiteSpace(SearchText) || t.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) 
            ).ToList();
            FilteredTransactions.Clear();
            foreach(var transaction in filteredTransactions)
            {
                FilteredTransactions.Add(transaction);
            }
        }

        private async Task EditTransaction(Transaction transaction)
        {
            if (EditingTransaction == null)
            {
                transaction.IsEditing = true;
                EditingTransaction = transaction;
            }
        }

        private async Task SaveTransaction(Transaction transaction)
        {
            transaction.IsEditing = false;
            EditingTransaction = null;
            await databaseService.UpdateTransaction(transaction);
            await LoadTransactions();
        }

        private async Task DeleteTransaction(Transaction transaction)
        {
            Transactions.Remove(transaction);
            FilterTransactions();
            await databaseService.DeleteTransaction(transaction);
        }
    }
}
