namespace Limita.Data.Repo.Implementation;

public class AccountRepo : IAccountRepo
{
    private readonly LimitaDbContext dbContext;

    public AccountRepo(LimitaDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<Account>> GetByUserIdAsync(int userId)
    {
        return await dbContext.Accounts
            .AsNoTracking()
            .Where(account => account.UserId == userId)
            .ToListAsync();
    }

    public async Task<Account?> GetByIdAndUserIdAsync(int accountId, int userId)
    {
        return await dbContext.Accounts
            .FirstOrDefaultAsync(account =>
                account.Id == accountId &&
                account.UserId == userId);
    }
}
