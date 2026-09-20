using Limita.Business.Common;
using Limita.Business.DTOs.Accounts;

namespace Limita.Business.Services.Interface;

public interface IAccountService
{
    Task<ServiceResult<List<AccountResponseDTO>>> GetAccountsAsync(int userId);
    Task<ServiceResult<AccountResponseDTO>> GetAccountAsync(int userId, int accountId);
}
