namespace Limita.Business.Services.Interface
{
    public interface IRegisterService
    {
        Task<ServiceResult<RegisterResponseDTO>>  RegisterAsync(RegisterRequestDTO request);
    }
}
