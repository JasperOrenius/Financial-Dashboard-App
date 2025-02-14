using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial_Dashboard_App.Commands
{
    public class RelayCommand : BaseCommand
    {
        private readonly Func<Task> execute;

        public RelayCommand(Func<Task> execute)
        {
            this.execute = execute;
        }

        public override async void Execute(object parameter)
        {
            if(execute != null)
            {
                await execute();
            }
        }
    }
}
