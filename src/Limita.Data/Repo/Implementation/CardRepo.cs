namespace Limita.Data.Repo.Implementation;

public class CardRepo : ICardRepo
{
    private readonly LimitaDbContext dbContext;

    public CardRepo(LimitaDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Card?> GetCardByIdAsync(int cardId)
    {
        return await dbContext.Cards
            .AsNoTracking()
            .Include(card => card.Account)
            .FirstOrDefaultAsync(card => card.Id == cardId);
    }

    public async Task<List<Card>> GetCardsByUserIdAsync(int userId)
    {
        return await dbContext.Cards
            .AsNoTracking()
            .Include(card => card.Account)
            .Where(card => card.Account.UserId == userId)
            .ToListAsync();
    }
}
