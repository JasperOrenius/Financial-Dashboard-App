using Financial_Dashboard_App.Models;
using Financial_Dashboard_App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Financial_Dashboard_App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly NavigationService navigationService;

        public BaseViewModel CurrentViewModel => navigationService.CurrentViewModel;

        public NavigationBarViewModel NavigationBarViewModel { get; }

        public MainViewModel(NavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;

            NavigationBarViewModel = new NavigationBarViewModel(
                navigationService,
                () => new DashboardViewModel(),
                () => new TransactionsViewModel(new ObservableCollection<Transaction>()),
                () => new ImportDataViewModel(),
                () => new ReportsViewModel()
            );
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
