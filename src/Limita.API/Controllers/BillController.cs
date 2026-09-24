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
    public async Task<ActionResult<List<BillResponseDTO>>> GetBills()
    {
        ServiceResult<List<BillResponseDTO>> result =
            await billService.GetBillsAsync(User.GetUserId());

        return result.ToActionResult(this);
    }

    [HttpGet("{billId:int}")]
    public async Task<ActionResult<BillResponseDTO>> GetBillById(int billId)
    {
        ServiceResult<BillResponseDTO> result =
            await billService.GetBillByIdAsync(
                User.GetUserId(),
                billId);

        return result.ToActionResult(this);
    }

    [HttpPost("{billId:int}/pay")]
    public async Task<ActionResult<PayBillResponseDTO>> PayBill(
        int billId,
        [FromBody] PayBillRequestDTO requestDTO)
    {
        ServiceResult<PayBillResponseDTO> result =
            await billService.PayBillAsync(
                User.GetUserId(),
                billId,
                requestDTO);

        return result.ToActionResult(this);
    }
}
