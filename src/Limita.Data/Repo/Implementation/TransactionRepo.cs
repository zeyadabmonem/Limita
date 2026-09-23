using Limita.Data.Entities;
using Limita.Data.Entities.Enums;
using Limita.Data.Repo.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Data.Repo.Implementation
{
    public class TransactionRepo : ITransactionRepo
    {
        private readonly LimitaDbContext dbContext;

        public TransactionRepo(LimitaDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }
        public async Task AddTransactionAsync(Transaction transaction)
        {
           await dbContext.Transactions.AddAsync(transaction);
           await dbContext.SaveChangesAsync();
        }

        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId ,TransactionType? type,
            TransactionStatus? status, DateTime? date)
        {
             return await dbContext.Transactions
                       .AsNoTracking()
                       .Include(t => t.Beneficiary)
                       .Where(t =>( t.UserId == userId) && (type == null || t.Type == type) && (status == null || t.Status == status) && (date == null ||t.CreatedAt == date))
                       .OrderByDescending(t => t.CreatedAt)
                       .ToListAsync();

        }

        public async Task<Transaction?> GetTransferAsync(int transactionId)
        {
             return await dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId && t.Type == Entities.Enums.TransactionType.Transfer);
        }

        public async Task<Transaction?> GetTransactionByIdAsync(
       int transactionId)
        {
            return await dbContext.Transactions
                .AsNoTracking()
                .Include(t => t.Beneficiary)
                .FirstOrDefaultAsync(t =>
                 t.Id == transactionId 
                 );
        }
    }
}
