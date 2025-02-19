using Financial_Dashboard_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial_Dashboard_App.Services
{
    public interface IDatabaseService
    {
        Task<IEnumerable<Transaction>> GetAllTransactions();
        Task CreateTransaction(Transaction transaction);
        Task UpdateTransaction(Transaction transaction);
        Task DeleteTransaction(Transaction transaction);
    }
}
