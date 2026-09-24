namespace Limita.Business.Services.Implementation;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepo transactionRepo;

    public TransactionService(ITransactionRepo transactionRepo)
    {
        this.transactionRepo = transactionRepo;
    }

    public async Task<ServiceResult<List<TransactionResponseDTO>>> GetTransactionsAsync(
        int userId,
        TransactionFilterDTO transactionFilter)
    {
        List<Transaction> transactions =
            await transactionRepo.GetTransactionsByUserIdAsync(
                userId,
                transactionFilter.Type,
                transactionFilter.Status,
                transactionFilter.Date);

        return new ServiceResult<List<TransactionResponseDTO>>
        {
            Success = true,
            Message = "Transactions retrieved successfully",
            Data = transactions.Select(MapToResponse).ToList()
        };
    }

    public async Task<ServiceResult<TransactionResponseDTO>> GetTransactionByIdAsync(
        int userId,
        int transactionId)
    {
        if (transactionId <= 0)
            return Failure<TransactionResponseDTO>(
                "Transaction id must be greater than zero",
                ServiceErrorCode.Validation);

        Transaction? transaction =
            await transactionRepo.GetTransactionByIdAsync(transactionId);

        if (transaction is null || transaction.UserId != userId)
            return Failure<TransactionResponseDTO>(
                "Transaction not found",
                ServiceErrorCode.NotFound);

        return new ServiceResult<TransactionResponseDTO>
        {
            Success = true,
            Message = "Transaction retrieved successfully",
            Data = MapToResponse(transaction)
        };
    }

    private static ServiceResult<T> Failure<T>(string message, ServiceErrorCode errorCode) =>
        new()
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode
        };

    private static TransactionResponseDTO MapToResponse(Transaction transaction) =>
        new()
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
