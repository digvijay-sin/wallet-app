using Expense_Tracker_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker_Data.Interface
{
    public interface ITransaction
    {
        Task<IEnumerable<TransactionRes>> GetTransactionsAsync();
        Task<TransactionRes?> UpdateTransactionAsync(int id, TransactionReq transaction);
        Task<bool> AddTransactionAsync(TransactionReq transaction);
        Task<TransactionRes?> GetTransactionAsync(int id);

        Task<bool> DeleteTransactionAsync(int id);
    }
}
