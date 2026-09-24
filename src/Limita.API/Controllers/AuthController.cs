namespace Limita.API.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IRegisterService registerService;
    private readonly ILoginService loginService;

    public AuthController(
        IRegisterService registerService,
        ILoginService loginService)
    {
        this.registerService = registerService;
        this.loginService = loginService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponseDTO>> Register(
        [FromBody] RegisterRequestDTO requestDTO)
    {
        ServiceResult<RegisterResponseDTO> result =
            await registerService.RegisterAsync(requestDTO);

        return result.ToActionResult(this);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login(
        [FromBody] LoginRequestDTO requestDTO)
    {
        ServiceResult<LoginResponseDTO> result =
            await loginService.LoginAsync(requestDTO);

        return result.ToActionResult(this);
    }
}
