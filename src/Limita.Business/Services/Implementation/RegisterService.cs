namespace Limita.Business.Services.Implementation;

public class RegisterService : IRegisterService
{
    private readonly IUserRepo userRepo;

    public RegisterService(IUserRepo userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task<ServiceResult<RegisterResponseDTO>> RegisterAsync(RegisterRequestDTO request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.PhoneNumber) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return Failure<RegisterResponseDTO>(
                "All registration fields are required",
                ServiceErrorCode.Validation);
        }

        if (await userRepo.ExistsByEmailOrPhoneAsync(request.Email, request.PhoneNumber))
        {
            return Failure<RegisterResponseDTO>(
                "Email or phone number is already in use",
                ServiceErrorCode.Conflict);
        }

        try
        {
            User user = new()
            {
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                FullName = request.FullName
            };

            int id = await userRepo.AddUserAsync(user);

            return new ServiceResult<RegisterResponseDTO>
            {
                Success = true,
                Message = "User created successfully",
                Data = new RegisterResponseDTO
                {
                    Id = id,
                    FullName = user.FullName,
                    Email = user.Email
                }
            };
        }
        catch (Exception)
        {
            return Failure<RegisterResponseDTO>(
                "Registration failed",
                ServiceErrorCode.Unexpected);
        }
    }

    private static ServiceResult<T> Failure<T>(string message, ServiceErrorCode errorCode) =>
        new()
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode
        };
}
