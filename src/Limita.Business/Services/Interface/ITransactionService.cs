namespace Limita.Business.Services.Interface
{
    public interface ITransactionService
    {
        Task<ServiceResult<List<TransactionResponseDTO>>> GetTransactionsAsync(int userId,TransactionFilterDTO transactionFilter);
         Task<ServiceResult<TransactionResponseDTO>>  GetTransactionByIdAsync(int userId, int transactionId);
    }
}
