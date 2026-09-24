namespace Limita.Data.Repo.Implementation
{
    public class CardRepo : ICardRepo
    {
        private readonly LimitaDbContext dbContext;

        public CardRepo(LimitaDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }
        public async Task<Card?> GetCardByIdAsync(int cardId)
        {
           return await dbContext.Cards.Include(c => c.Account).FirstOrDefaultAsync(c => c.Id == cardId);
        }

        public async Task<List<Card>> GetCardsByUserIdAsync(int userId)
        {
            return await dbContext.Cards
                .Include(c => c.Account)
                .Where(c => c.Account.UserId == userId)
                .ToListAsync();
        }
    }
}
