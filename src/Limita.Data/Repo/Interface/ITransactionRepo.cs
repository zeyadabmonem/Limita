using Limita.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Data.Repo.Interface
{
    public interface ITransactionRepo
    {
        Task AddTransactionAsync(Transaction transaction);
        Task<Transaction?> GetTransferAsync(int transactionId);
    }
}
