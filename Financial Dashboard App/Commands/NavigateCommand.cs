using Financial_Dashboard_App.Services;
using Financial_Dashboard_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial_Dashboard_App.Commands
{
    public class NavigateCommand : BaseCommand
    {
        private readonly NavigationService navigationService;
        private readonly Func<BaseViewModel> createViewModel;

        public NavigateCommand(NavigationService navigationService, Func<BaseViewModel> createViewModel)
        {
            this.navigationService = navigationService;
            this.createViewModel = createViewModel;
        }

        public override void Execute(object parameter)
        {
            navigationService.CurrentViewModel = createViewModel();
        }
    }
}
