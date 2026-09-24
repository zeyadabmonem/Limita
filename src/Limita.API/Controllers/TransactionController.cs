namespace Limita.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/transactions")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        this.transactionService = transactionService;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(List<TransactionResponseDTO>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<TransactionResponseDTO>>> GetTransactions([FromQuery] TransactionFilterDTO transactionFilter)
    {
        int userId = User.GetUserId();

        var response = await transactionService.GetTransactionsAsync(userId, transactionFilter);

        if (!response.Success)
        {
            return BadRequest(response.Message);
        }

        return Ok(response.Data);
    }

    [HttpGet("{transactionId:int}")]
    public async Task<
       ActionResult<TransactionResponseDTO>>
       GetTransactionById(int transactionId)
    {
        int userId = User.GetUserId();

        var response =
            await transactionService.GetTransactionByIdAsync(
                userId,
                transactionId);

        if (!response.Success)
        {
            return BadRequest(response.Message);
        }

        return Ok(response.Data);
    }
}
