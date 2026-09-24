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
            .Where(bill => bill.UserId == userId)
            .OrderByDescending(bill => bill.DueDate)
            .ToListAsync();
    }

    public async Task<Bill?> GetByIdAndUserIdAsync(int billId, int userId)
    {
        return await dbContext.Bills
            .FirstOrDefaultAsync(bill =>
                bill.Id == billId &&
                bill.UserId == userId);
    }
}
