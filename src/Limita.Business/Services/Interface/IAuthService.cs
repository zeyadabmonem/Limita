namespace Limita.Business.Services.Interface;

public interface IAuthService
{
    Task<ServiceResult<bool>> ChangePasswordAsync(int userId, ChangePasswordRequestDTO request);
}
