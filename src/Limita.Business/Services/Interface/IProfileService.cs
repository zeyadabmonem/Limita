using Limita.Business.Common;
using Limita.Business.DTOs.Profile;

namespace Limita.Business.Services.Interface;

public interface IProfileService
{
    Task<ServiceResult<ProfileResponseDTO>> GetProfileAsync(int userId);
    Task<ServiceResult<ProfileResponseDTO>> UpdateProfileAsync(int userId, UpdateProfileRequestDTO request);
}
