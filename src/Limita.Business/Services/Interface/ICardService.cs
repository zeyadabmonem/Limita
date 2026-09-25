namespace Limita.Business.Services.Interface;

public interface ICardService
{
    Task<ServiceResult<List<CardResponseDTO>>> GetAll(int userId);
    Task<ServiceResult<CardResponseDTO>> GetById(int userId, int cardId);
    Task<ServiceResult<CardResponseDTO>> AddAsync(int userId, AddCardRequestDTO request);
    Task<ServiceResult<CardResponseDTO>> UpdateStatusAsync(int userId, int cardId, CardStatus status);
}
