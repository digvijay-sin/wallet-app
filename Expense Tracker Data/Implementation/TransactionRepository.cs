﻿using AutoMapper;
using Expense_Tracker_Core.Models;
using Expense_Tracker_Data.Context;
using Expense_Tracker_Data.Interface;
using Expense_Tracker_Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Expense_Tracker_Data.Implementation
{
    public class TransactionRepository : ITransaction
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TransactionRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddTransactionAsync(TransactionReq transaction, int userId)
        {
            var newTransaction = _mapper.Map<Transaction>(transaction);
            newTransaction.UserId = userId;
            await _context.AddAsync(newTransaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTransactionAsync(int id, int userId)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if(transaction == null)
            {
                return false;
            }

            if (transaction.UserId != userId)
            {
                throw new Exception("Unauthorized");
            }
            _context.Remove(transaction);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<TransactionRes?> GetTransactionAsync(int id, int userId)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return null;
            }

            if(transaction.UserId != userId)
            {
                throw new Exception("Unauthorized");
            }

            var _transaction = _mapper.Map<TransactionRes>(transaction);
            return _transaction;
        }

        public async Task<IEnumerable<TransactionRes>> GetTransactionsAsync(int userId)
        {
            var transactions = await _context.Transactions.Where(c => c.UserId == userId).Include(t => t.Category).ToListAsync();
            
            List<TransactionRes> transactionRes = new List<TransactionRes>();
            foreach (var transaction in transactions)
            {
                transactionRes.Add(_mapper.Map<TransactionRes>(transaction));
            }
            return transactionRes;
        }

        public async Task<TransactionRes?> UpdateTransactionAsync(int id, TransactionReq modifiedTransaction, int userId)
        {
            var _transaction = await _context.Transactions.FindAsync(id);
            if (_transaction == null)
            {
                return null;
            }

            if(_transaction.UserId != userId)
            {
                throw new Exception("Unauthorized");
            }

            _context.Entry(_transaction).CurrentValues.SetValues(modifiedTransaction);
            _context.Entry(_transaction).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            var transaction = _mapper.Map<TransactionRes>(_transaction);

            return transaction;
        }
    }
}
