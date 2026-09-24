namespace Limita.Data.Repo.Interface;

public interface IAccountRepo
{
    Task<List<Account>> GetByUserIdAsync(int userId);
    Task<Account?> GetByIdAndUserIdAsync(int accountId, int userId);
}
