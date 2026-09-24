namespace Limita.Business.Services.Interface
{
    public interface ICardService
    {
        Task<ServiceResult<List<CardResponseDTO>>> GetAll(int userId);
        Task<ServiceResult<CardResponseDTO>> GetById(int userId,int cardId);

    }
}
