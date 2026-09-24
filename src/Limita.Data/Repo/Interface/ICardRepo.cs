namespace Limita.Data.Repo.Interface
{
    public interface ICardRepo
    {
        Task<List<Card>> GetCardsByUserIdAsync(int userId);
        Task<Card?> GetCardByIdAsync(int cardId);


    }
}
