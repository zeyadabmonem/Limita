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

        if (user is null)
            return Failure<ProfileResponseDTO>("User not found", ServiceErrorCode.NotFound);

        return new ServiceResult<ProfileResponseDTO>
        {
            Success = true,
            Message = "Profile retrieved successfully",
            Data = MapToResponse(user)
        };
    }

    public async Task<ServiceResult<ProfileResponseDTO>> UpdateProfileAsync(
        int userId,
        UpdateProfileRequestDTO request)
    {
        User? user = await userRepo.GetByIdAsync(userId);

        if (user is null)
            return Failure<ProfileResponseDTO>("User not found", ServiceErrorCode.NotFound);

        if (string.IsNullOrWhiteSpace(request.FullName) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            return Failure<ProfileResponseDTO>(
                "Full name, email and phone number are required",
                ServiceErrorCode.Validation);
        }

        bool detailsUsedByAnotherUser =
            await userRepo.ExistsByEmailOrPhoneForOtherUserAsync(
                userId,
                request.Email,
                request.PhoneNumber);

        if (detailsUsedByAnotherUser)
            return Failure<ProfileResponseDTO>(
                "Email or phone number is already in use",
                ServiceErrorCode.Conflict);

        user.FullName = request.FullName;
        user.Email = request.Email;
        user.PhoneNumber = request.PhoneNumber;
        user.ProfileImage = request.ProfileImage;
        user.Language = request.Language;
        user.Currency = request.Currency;
        user.UpdatedAt = DateTime.UtcNow;

        await userRepo.UpdateAsync(user);

        return new ServiceResult<ProfileResponseDTO>
        {
            Success = true,
            Message = "Profile updated successfully",
            Data = MapToResponse(user)
        };
    }

    private static ServiceResult<T> Failure<T>(string message, ServiceErrorCode errorCode) =>
        new()
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode
        };

    private static ProfileResponseDTO MapToResponse(User user) =>
        new()
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
