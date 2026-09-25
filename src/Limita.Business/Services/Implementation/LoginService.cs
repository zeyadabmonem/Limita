namespace Limita.Business.Services.Implementation;

public class LoginService : ILoginService
{
    private readonly IUserRepo userRepo;
    private readonly ITokenService tokenService;

    public LoginService(IUserRepo userRepo, ITokenService tokenService)
    {
        this.userRepo = userRepo;
        this.tokenService = tokenService;
    }

    public async Task<ServiceResult<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequest)
    {
        User? user = await userRepo.GetByEmailAsync(loginRequest.Email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
        {
            return new ServiceResult<LoginResponseDTO>
            {
                Success = false,
                Message = "Invalid email or password",
                ErrorCode = ServiceErrorCode.Unauthorized
            };
        }

        string token = tokenService.CreateToken(user.Id);

        return new ServiceResult<LoginResponseDTO>
        {
            Success = true,
            Message = "Login successful",
            Data = new LoginResponseDTO
            {
                FullName = user.FullName,
                Id = user.Id,
                Token = token
            }
        };
    }
}
