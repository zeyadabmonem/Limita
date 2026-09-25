namespace Limita.API.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IRegisterService registerService;
    private readonly ILoginService loginService;
    private readonly IAuthService authService;

    public AuthController(IRegisterService registerService, ILoginService loginService, IAuthService authService)
    {
        this.registerService = registerService;
        this.loginService = loginService;
        this.authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO requestDTO)
    {
        ServiceResult<RegisterResponseDTO> result = await registerService.RegisterAsync(requestDTO);
        return result.ToActionResult(this);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO requestDTO)
    {
        ServiceResult<LoginResponseDTO> result = await loginService.LoginAsync(requestDTO);
        return result.ToActionResult(this);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDTO requestDTO)
    {
        ServiceResult<bool> result = await authService.ChangePasswordAsync(User.GetUserId(), requestDTO);
        return result.ToActionResult(this);
    }
}
