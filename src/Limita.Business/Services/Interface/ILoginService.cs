namespace Limita.Business.Services.Interface
{
    public interface ILoginService
    {
        Task<ServiceResult<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequest);
    }
}
