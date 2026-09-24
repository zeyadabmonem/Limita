namespace Limita.API.Controllers;

[Authorize]
[Route("api/v1/accounts")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService accountService;

    public AccountController(IAccountService accountService)
    {
        this.accountService = accountService;
    }

    [HttpGet]
    public async Task<ActionResult<List<AccountResponseDTO>>> GetAccounts()
    {
        int? userId = GetAuthenticatedUserId();
        if (userId == null)
            return Unauthorized();

        ServiceResult<List<AccountResponseDTO>> result = await accountService.GetAccountsAsync(userId.Value);
        return Ok(result.Data);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AccountResponseDTO>> GetAccount(int id)
    {
        int? userId = GetAuthenticatedUserId();
        if (userId == null)
            return Unauthorized();

        ServiceResult<AccountResponseDTO> result = await accountService.GetAccountAsync(userId.Value, id);
        if (!result.Success)
            return NotFound(result.Message);

        return Ok(result.Data);
    }

    private int? GetAuthenticatedUserId()
    {
        string? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out int userId) ? userId : null;
    }
}
