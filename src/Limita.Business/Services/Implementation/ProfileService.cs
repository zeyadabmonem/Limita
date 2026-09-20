using Limita.Business.Common;
using Limita.Business.DTOs.Profile;
using Limita.Business.Services.Interface;
using Limita.Data.Entities;
using Limita.Data.Repo.Interface;

namespace Limita.Business.Services.Implementation;

public class ProfileService : IProfileService
{
    private readonly IUserRepo userRepo;

    public ProfileService(IUserRepo userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task<ServiceResult<ProfileResponseDTO>> GetProfileAsync(int userId)
    {
        User? user = await userRepo.GetByIdAsync(userId);
        if (user == null)
            return new ServiceResult<ProfileResponseDTO> { Success = false, Message = "User not found" };

        return new ServiceResult<ProfileResponseDTO> { Success = true, Data = MapToResponse(user) };
    }

    public async Task<ServiceResult<ProfileResponseDTO>> UpdateProfileAsync(int userId, UpdateProfileRequestDTO request)
    {
        User? user = await userRepo.GetByIdAsync(userId);
        if (user == null)
            return new ServiceResult<ProfileResponseDTO> { Success = false, Message = "User not found" };

        bool detailsUsedByAnotherUser = await userRepo.ExistsByEmailOrPhoneForOtherUserAsync(userId, request.Email, request.PhoneNumber);
        if (detailsUsedByAnotherUser)
            return new ServiceResult<ProfileResponseDTO> { Success = false, Message = "Email or phone number is already in use" };

        user.FullName = request.FullName;
        user.Email = request.Email;
        user.PhoneNumber = request.PhoneNumber;
        user.ProfileImage = request.ProfileImage;
        user.Language = request.Language;
        user.Currency = request.Currency;
        user.UpdatedAt = DateTime.UtcNow;

        await userRepo.UpdateAsync(user);

        return new ServiceResult<ProfileResponseDTO> { Success = true, Data = MapToResponse(user), Message = "Profile updated successfully" };
    }

    private static ProfileResponseDTO MapToResponse(User user)
    {
        return new ProfileResponseDTO
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            ProfileImage = user.ProfileImage,
            Language = user.Language,
            Currency = user.Currency
        };
    }
}
