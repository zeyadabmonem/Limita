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
    public async Task<IActionResult> GetAccounts()
    {
        ServiceResult<List<AccountResponseDTO>> result =
            await accountService.GetAccountsAsync(User.GetUserId());

        return result.ToActionResult(this);
    }

    [HttpGet("{accountId:int}")]
    public async Task<IActionResult> GetAccount(int accountId)
    {
        ServiceResult<AccountResponseDTO> result =
            await accountService.GetAccountAsync(User.GetUserId(), accountId);

        return result.ToActionResult(this);
    }
}
