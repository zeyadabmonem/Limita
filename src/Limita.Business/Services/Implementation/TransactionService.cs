using Limita.Business.Common;
using Limita.Business.DTOs.Transaction;
using Limita.Business.DTOs.Transactions;
using Limita.Business.Services.Interface;
using Limita.Data.Entities;
using Limita.Data.Repo.Interface;

namespace Limita.Business.Services.Implementation;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepo transactionRepo;

    public TransactionService(ITransactionRepo transactionRepo)
    {
        this.transactionRepo = transactionRepo;
    }

    public async Task<ServiceResult<List<TransactionResponseDTO>>> GetTransactionsAsync(
        int userId,TransactionFilterDTO transactionFilter)
    {
        List<Transaction> transactions =
            await transactionRepo.GetTransactionsByUserIdAsync(userId, transactionFilter.Type, transactionFilter.Status, transactionFilter.Date);

        List<TransactionResponseDTO> response =
            transactions.Select(MapToResponse).ToList();

        return new ServiceResult<List<TransactionResponseDTO>>
        {
            Success = true,
            Message = "Transactions retrieved successfully",
            Data = response
        };
    }


    public async Task<ServiceResult<TransactionResponseDTO>>
      GetTransactionByIdAsync(
          int userId,
          int transactionId)
    {



        Transaction? transaction =
            await transactionRepo.GetTransactionByIdAsync(
                transactionId     );


        if (transaction is null || userId != transaction.UserId)
        {
            return new ServiceResult<TransactionResponseDTO>
            {
                Success = false,
                Message = "Transaction not found"
            };
        }

        return new ServiceResult<TransactionResponseDTO>
        {
            Success = true,
            Message = "Transaction retrieved successfully",
            Data = MapToResponse(transaction)
        };
    }

    private static TransactionResponseDTO MapToResponse(
        Transaction transaction)
    {
        return new TransactionResponseDTO
        {
             Id = transaction.Id,
            AccountId = transaction.AccountId,
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            Type = transaction.Type,
            Status = transaction.Status,
            Reference = transaction.Reference,
            CreatedAt = transaction.CreatedAt,
            BeneficiaryName = transaction.Beneficiary?.Name
        };
    }
}
