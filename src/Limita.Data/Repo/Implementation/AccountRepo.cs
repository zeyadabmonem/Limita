using Limita.Data.Entities;
using Limita.Data.Repo.Interface;
using Microsoft.EntityFrameworkCore;

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
            
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }

    public async Task<Account?> GetByIdAndUserIdAsync(int accountId, int userId)
    {
        return await dbContext.Accounts
          
            .FirstOrDefaultAsync(a => a.Id == accountId && a.UserId == userId);
    }

   
}
