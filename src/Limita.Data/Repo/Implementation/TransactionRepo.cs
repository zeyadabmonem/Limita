namespace Limita.Data.Repo.Implementation;

public class TransactionRepo : ITransactionRepo
{
    private readonly LimitaDbContext dbContext;

    public TransactionRepo(LimitaDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        await dbContext.Transactions.AddAsync(transaction);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<Transaction>> GetTransactionsByUserIdAsync(
        int userId,
        TransactionType? type,
        TransactionStatus? status,
        DateTime? date)
    {
        return await dbContext.Transactions
            .AsNoTracking()
            .Include(transaction => transaction.Beneficiary)
            .Where(transaction =>
                transaction.UserId == userId &&
                (type == null || transaction.Type == type) &&
                (status == null || transaction.Status == status) &&
                (date == null || transaction.CreatedAt == date))
            .OrderByDescending(transaction => transaction.CreatedAt)
            .ToListAsync();
    }

    public async Task<Transaction?> GetTransferAsync(int transactionId)
    {
        return await dbContext.Transactions
            .AsNoTracking()
            .FirstOrDefaultAsync(transaction =>
                transaction.Id == transactionId &&
                transaction.Type == TransactionType.Transfer);
    }

    public async Task<Transaction?> GetTransactionByIdAsync(int transactionId)
    {
        return await dbContext.Transactions
            .AsNoTracking()
            .Include(transaction => transaction.Beneficiary)
            .FirstOrDefaultAsync(transaction => transaction.Id == transactionId);
    }
}
