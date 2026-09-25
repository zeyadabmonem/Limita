namespace Limita.Business.Services.Interface;

public interface IProfileService
{
    Task<ServiceResult<ProfileResponseDTO>> GetProfileAsync(int userId);
    Task<ServiceResult<ProfileResponseDTO>> UpdateProfileAsync(int userId, UpdateProfileRequestDTO request);
}
