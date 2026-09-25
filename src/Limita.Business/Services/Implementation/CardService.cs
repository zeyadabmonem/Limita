namespace Limita.Business.Services.Implementation;

public class CardService : ICardService
{
    private readonly ICardRepo cardRepo;
    private readonly IAccountRepo accountRepo;

    public CardService(ICardRepo cardRepo, IAccountRepo accountRepo)
    {
        this.cardRepo = cardRepo;
        this.accountRepo = accountRepo;
    }

    public async Task<ServiceResult<List<CardResponseDTO>>> GetAll(int userId)
    {
        List<Card> cards = await cardRepo.GetCardsByUserIdAsync(userId);
        return new ServiceResult<List<CardResponseDTO>> { Success = true, Message = "Cards retrieved successfully", Data = cards.Select(MapToResponse).ToList() };
    }

    public async Task<ServiceResult<CardResponseDTO>> GetById(int userId, int cardId)
    {
        if (cardId <= 0) return Failure<CardResponseDTO>("Card id must be greater than zero", ServiceErrorCode.Validation);
        Card? card = await cardRepo.GetCardByIdAsync(cardId);
        if (card is null || card.Account.UserId != userId) return Failure<CardResponseDTO>("Card not found", ServiceErrorCode.NotFound);
        return new ServiceResult<CardResponseDTO> { Success = true, Message = "Card retrieved successfully", Data = MapToResponse(card) };
    }

    public async Task<ServiceResult<CardResponseDTO>> AddAsync(int userId, AddCardRequestDTO request)
    {
        Account? account = await accountRepo.GetByIdAndUserIdAsync(request.AccountId, userId);
        if (account is null) return Failure<CardResponseDTO>("Account not found", ServiceErrorCode.NotFound);
        if (request.ExpiryDate <= DateOnly.FromDateTime(DateTime.UtcNow)) return Failure<CardResponseDTO>("Card expiry date must be in the future", ServiceErrorCode.Validation);
        Card card = new() { AccountId = account.Id, CardName = request.CardName.Trim(), LastFourDigits = request.LastFourDigits, Brand = request.Brand.Trim(), ExpiryDate = request.ExpiryDate, Status = CardStatus.Active, CreatedAt = DateTime.UtcNow };
        await cardRepo.AddAsync(card);
        return new ServiceResult<CardResponseDTO> { Success = true, Message = "Card added successfully", Data = MapToResponse(card) };
    }

    public async Task<ServiceResult<CardResponseDTO>> UpdateStatusAsync(int userId, int cardId, CardStatus status)
    {
        if (cardId <= 0) return Failure<CardResponseDTO>("Card id must be greater than zero", ServiceErrorCode.Validation);
        if (status == CardStatus.Expired) return Failure<CardResponseDTO>("Expired is not a user-selectable card status", ServiceErrorCode.Validation);
        Card? card = await cardRepo.GetCardByIdAsync(cardId);
        if (card is null || card.Account.UserId != userId) return Failure<CardResponseDTO>("Card not found", ServiceErrorCode.NotFound);
        card.Status = status;
        await cardRepo.UpdateAsync(card);
        return new ServiceResult<CardResponseDTO> { Success = true, Message = "Card status updated successfully", Data = MapToResponse(card) };
    }

    private static ServiceResult<T> Failure<T>(string message, ServiceErrorCode errorCode) => new() { Success = false, Message = message, ErrorCode = errorCode };
    private static CardResponseDTO MapToResponse(Card card) => new() { Id = card.Id, Brand = card.Brand, CardName = card.CardName, ExpiryDate = card.ExpiryDate, LastFourDigits = card.LastFourDigits, Status = card.Status };
}
