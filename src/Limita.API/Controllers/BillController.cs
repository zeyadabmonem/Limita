namespace Limita.API.Controllers;

[Authorize]
[Route("api/v1/bills")]
[ApiController]
public class BillController : ControllerBase
{
    private readonly IBillService billService;

    public BillController(IBillService billService)
    {
        this.billService = billService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<BillResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<BillResponseDTO>>> GetBills()
    {
        var response = await billService.GetBillsAsync(User.GetUserId());

        if (!response.Success)
            return BadRequest(response.Message);

        return Ok(response.Data);
    }

    [HttpGet("{billId:int}")]
    [ProducesResponseType(typeof(BillResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BillResponseDTO>> GetBillById(int billId)
    {
        var response = await billService.GetBillByIdAsync(User.GetUserId(), billId);

        if (!response.Success)
            return BadRequest(response.Message);

        return Ok(response.Data);
    }

    [HttpPost("{billId:int}/pay")]
    [ProducesResponseType(typeof(PayBillResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PayBillResponseDTO>> PayBill(int billId, [FromBody] PayBillRequestDTO requestDTO)
    {
        var response = await billService.PayBillAsync(User.GetUserId(), billId, requestDTO);

        if (!response.Success)
            return BadRequest(response.Message);

        return Ok(response.Data);
    }
}