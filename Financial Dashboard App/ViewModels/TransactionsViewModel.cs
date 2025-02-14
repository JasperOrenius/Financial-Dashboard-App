using Financial_Dashboard_App.Models;
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
        public ObservableCollection<Transaction> Transactions { get; set; }
        public ObservableCollection<Transaction> FilteredTransactions { get; set; }
        public List<string> TransactionTypes { get; } = new List<string> { "All", "Income", "Expenses" };

        private string selectedFilter = "All";
        public string SelectedFilter
        {
            get => selectedFilter;
            set
            {
                selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
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
            }
        }

        public ICommand EditCommand;
        public ICommand DeleteCommand;

        public TransactionsViewModel(ObservableCollection<Transaction> transactions)
        {
            Transactions = transactions;
            FilteredTransactions = new ObservableCollection<Transaction>(transactions);
        }

        private void FilterTransactions()
        {
            var filteredTransactions = Transactions.Where(transaction =>
                (SelectedFilter == "All" || transaction.Type == selectedFilter) &&
                (string.IsNullOrWhiteSpace(SearchText) || transaction.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) 
            ).ToList();
            FilteredTransactions.Clear();
            foreach(var transaction in filteredTransactions)
            {
                FilteredTransactions.Add(transaction);
            }
        }

        private void EditTransaction(object parameter)
        {
            if(parameter is Transaction transaction)
            {
                MessageBox.Show("Edit");
            }
        }

        private void DeleteTransaction(object parameter)
        {
            if(parameter is Transaction transaction)
            {
                Transactions.Remove(transaction);
                FilterTransactions();
            }
        }
    }
}
