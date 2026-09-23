using Limita.Business.Common;
using Limita.Business.DTOs.Transaction;
using Limita.Business.DTOs.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Business.Services.Interface
{
    public interface ITransactionService
    {
        Task<ServiceResult<List<TransactionResponseDTO>>> GetTransactionsAsync(int userId,TransactionFilterDTO transactionFilter);
         Task<ServiceResult<TransactionResponseDTO>>  GetTransactionByIdAsync(int userId, int transactionId);
    }
}
