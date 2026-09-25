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
    public async Task<IActionResult> GetTransactions(
        [FromQuery] TransactionFilterDTO transactionFilter)
    {
        ServiceResult<List<TransactionResponseDTO>> result =
            await transactionService.GetTransactionsAsync(
                User.GetUserId(),
                transactionFilter);

        return result.ToActionResult(this);
    }

    [HttpGet("{transactionId:int}")]
    public async Task<IActionResult> GetTransactionById(
        int transactionId)
    {
        ServiceResult<TransactionResponseDTO> result =
            await transactionService.GetTransactionByIdAsync(
                User.GetUserId(),
                transactionId);

        return result.ToActionResult(this);
    }
}
