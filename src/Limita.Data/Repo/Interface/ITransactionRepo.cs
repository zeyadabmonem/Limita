using Limita.Data.Entities;
using Limita.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Data.Repo.Interface
{
    public interface ITransactionRepo
    {
        Task AddTransactionAsync(Transaction transaction);
        Task<Transaction?> GetTransferAsync(int transactionId);

        Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId , TransactionType? type,
            TransactionStatus? status, DateTime? date);

        Task<Transaction?> GetTransactionByIdAsync(  int transactionId);

    }
}
