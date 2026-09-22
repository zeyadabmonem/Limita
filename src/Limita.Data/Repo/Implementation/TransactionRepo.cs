using Limita.Data.Entities;
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

        public async Task<Transaction?> GetTransferAsync(int transactionId)
        {
             return await dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId && t.Type == Entities.Enums.TransactionType.Transfer);
        }
    }
}
