using Financial_Dashboard_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial_Dashboard_App.Commands
{
    public class SaveTransactionCommand : BaseCommand
    {
        private readonly Func<Transaction, Task> execute;

        public SaveTransactionCommand(Func<Transaction, Task> execute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public override async void Execute(object parameter)
        {
            if(parameter is Transaction transaction)
            {
                await execute(transaction);
            }
        }
    }
}
