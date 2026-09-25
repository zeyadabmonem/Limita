namespace Limita.Data.Repo.Implementation;

public class CardRepo : ICardRepo
{
    private readonly LimitaDbContext dbContext;
    public CardRepo(LimitaDbContext dbContext) { this.dbContext = dbContext; }
    public async Task<Card?> GetCardByIdAsync(int cardId) => await dbContext.Cards.Include(card => card.Account).FirstOrDefaultAsync(card => card.Id == cardId);
    public async Task<List<Card>> GetCardsByUserIdAsync(int userId) => await dbContext.Cards.AsNoTracking().Include(card => card.Account).Where(card => card.Account.UserId == userId).ToListAsync();
    public async Task AddAsync(Card card) { await dbContext.Cards.AddAsync(card); await dbContext.SaveChangesAsync(); }
    public async Task UpdateAsync(Card card) { await dbContext.SaveChangesAsync(); }
}
