namespace Limita.Business.Services.Implementation;

public class AccountService : IAccountService
{
    private readonly IAccountRepo accountRepo;

    public AccountService(IAccountRepo accountRepo)
    {
        this.accountRepo = accountRepo;
    }

    public async Task<ServiceResult<List<AccountResponseDTO>>> GetAccountsAsync(int userId)
    {
        List<Account> accounts = await accountRepo.GetByUserIdAsync(userId);

        return new ServiceResult<List<AccountResponseDTO>>
        {
            Success = true,
            Message = "Accounts retrieved successfully",
            Data = accounts.Select(MapToResponse).ToList()
        };
    }

    public async Task<ServiceResult<AccountResponseDTO>> GetAccountAsync(int userId, int accountId)
    {
        if (accountId <= 0)
            return Failure<AccountResponseDTO>("Account id must be greater than zero", ServiceErrorCode.Validation);

        Account? account = await accountRepo.GetByIdAndUserIdAsync(accountId, userId);

        if (account is null)
            return Failure<AccountResponseDTO>("Account not found", ServiceErrorCode.NotFound);

        return new ServiceResult<AccountResponseDTO>
        {
            Success = true,
            Message = "Account retrieved successfully",
            Data = MapToResponse(account)
        };
    }

    private static ServiceResult<T> Failure<T>(string message, ServiceErrorCode errorCode) =>
        new()
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode
        };

    private static AccountResponseDTO MapToResponse(Account account) =>
        new()
        {
            Id = account.Id,
            Balance = account.Balance,
            Currency = account.Currency,
            Status = account.Status.ToString()
        };
}
