namespace Limita.Business.Services.Implementation;

public class AuthService : IAuthService
{
    private readonly IUserRepo userRepo;

    public AuthService(IUserRepo userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task<ServiceResult<bool>> ChangePasswordAsync(
        int userId,
        ChangePasswordRequestDTO request)
    {
        User? user = await userRepo.GetByIdAsync(userId);

        if (user is null)
            return Failure<bool>("User not found", ServiceErrorCode.NotFound);

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            return Failure<bool>("Current password is incorrect", ServiceErrorCode.Unauthorized);

        if (BCrypt.Net.BCrypt.Verify(request.NewPassword, user.PasswordHash))
            return Failure<bool>("New password must be different from the current password", ServiceErrorCode.Validation);

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await userRepo.UpdateAsync(user);

        return new ServiceResult<bool>
        {
            Success = true,
            Message = "Password changed successfully",
            Data = true
        };
    }

    private static ServiceResult<T> Failure<T>(string message, ServiceErrorCode errorCode) =>
        new()
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode
        };
}
