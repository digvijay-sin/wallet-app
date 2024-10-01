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
        Task<IEnumerable<TransactionRes>> GetTransactionsAsync(int userId);
        Task<TransactionRes?> UpdateTransactionAsync(int id, TransactionReq transaction, int userId);
        Task<bool> AddTransactionAsync(TransactionReq transaction, int userId);
        Task<TransactionRes?> GetTransactionAsync(int id, int userId);

        Task<bool> DeleteTransactionAsync(int id, int userId);
    }
}
