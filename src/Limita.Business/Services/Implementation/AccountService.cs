using Limita.Business.Common;
using Limita.Business.DTOs.Accounts;
using Limita.Business.Services.Interface;
using Limita.Data.Entities;
using Limita.Data.Repo.Interface;

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
            Data = accounts.Select(MapToResponse).ToList()
        };
    }

    public async Task<ServiceResult<AccountResponseDTO>> GetAccountAsync(int userId, int accountId)
    {
        Account? account = await accountRepo.GetByIdAndUserIdAsync(accountId, userId);
        if (account == null)
            return new ServiceResult<AccountResponseDTO> { Success = false, Message = "Account not found" };

        return new ServiceResult<AccountResponseDTO> { Success = true, Data = MapToResponse(account) };
    }

    private static AccountResponseDTO MapToResponse(Account account)
    {
        return new AccountResponseDTO
        {
            Id = account.Id,
            Balance = account.Balance,
            Currency = account.Currency,
            Status = account.Status.ToString()
        };
    }
}
