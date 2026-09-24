namespace Limita.Business.Services.Implementation;

public class CardService : ICardService
{
    private readonly ICardRepo cardRepo;

    public CardService(ICardRepo cardRepo)
    {
        this.cardRepo = cardRepo;
    }

    public async Task<ServiceResult<List<CardResponseDTO>>> GetAll(int userId)
    {
        List<Card> cards = await cardRepo.GetCardsByUserIdAsync(userId);

        return new ServiceResult<List<CardResponseDTO>>
        {
            Success = true,
            Message = "Cards retrieved successfully",
            Data = cards.Select(MapToResponse).ToList()
        };
    }

    public async Task<ServiceResult<CardResponseDTO>> GetById(int userId, int cardId)
    {
        if (cardId <= 0)
            return Failure<CardResponseDTO>("Card id must be greater than zero", ServiceErrorCode.Validation);

        Card? card = await cardRepo.GetCardByIdAsync(cardId);

        if (card is null || card.Account.UserId != userId)
            return Failure<CardResponseDTO>("Card not found", ServiceErrorCode.NotFound);

        return new ServiceResult<CardResponseDTO>
        {
            Success = true,
            Message = "Card retrieved successfully",
            Data = MapToResponse(card)
        };
    }

    private static ServiceResult<T> Failure<T>(string message, ServiceErrorCode errorCode) =>
        new()
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode
        };

    private static CardResponseDTO MapToResponse(Card card) =>
        new()
        {
            Id = card.Id,
            Brand = card.Brand,
            CardName = card.CardName,
            ExpiryDate = card.ExpiryDate,
            LastFourDigits = card.LastFourDigits,
            Status = card.Status
        };
}
