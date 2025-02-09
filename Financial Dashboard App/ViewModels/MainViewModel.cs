﻿using Financial_Dashboard_App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                () => new TransactionsViewModel(),
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
