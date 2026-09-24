namespace Limita.Data.Repo.Implementation;

public class BillRepo : IBillRepo
{
    private readonly LimitaDbContext dbContext;

    public BillRepo(LimitaDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<Bill>> GetByUserIdAsync(int userId)
    {
        return await dbContext.Bills
            .AsNoTracking()
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.DueDate)
            .ToListAsync();
    }

    public async Task<Bill?> GetByIdAndUserIdAsync(int billId, int userId)
    {
        return await dbContext.Bills
            .FirstOrDefaultAsync(b => b.Id == billId && b.UserId == userId);
    }
}