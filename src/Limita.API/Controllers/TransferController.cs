namespace Limita.API.Controllers;

[Authorize]
[Route("api/v1/transfers")]
[ApiController]
public class TransferController : ControllerBase
{
    private readonly ITransferService transferService;

    public TransferController(ITransferService transferService)
    {
        this.transferService = transferService;
    }

    [HttpPost]
    public async Task<ActionResult<TransferResponseDTO>> CreateTransfer(
        [FromBody] TransferRequestDTO requestDTO)
    {
        ServiceResult<TransferResponseDTO> result =
            await transferService.AddNewTransferAsync(
                User.GetUserId(),
                requestDTO);

        return result.ToActionResult(this);
    }

    [HttpGet("{transferId:int}")]
    public async Task<ActionResult<TransferResponseDTO>> GetTransfer(int transferId)
    {
        ServiceResult<TransferResponseDTO> result =
            await transferService.GetTransferByIdAsync(
                User.GetUserId(),
                transferId);

        return result.ToActionResult(this);
    }
}
