namespace Limita.API.Controllers
{
    [Authorize]
    [Route("api/v1/transfers")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService transferService;

        public TransferController (ITransferService transferService)
        {
            this.transferService = transferService;
        }
        [HttpPost]
        public async Task<ActionResult<TransferResponseDTO>> CreateTransfer([FromBody] TransferRequestDTO requestDTO)
        {
            var response = await transferService.AddNewTransferAsync(User.GetUserId(), requestDTO);

            if(response.Success)
                return Ok(response.Data);

            return BadRequest(response.Message);
        }
        [HttpGet("{transferId}")]
        public async Task<ActionResult<TransferResponseDTO>> GetTranfer(int transferId)
        {
            var response = await transferService.GetTransferByIdAsync(User.GetUserId(), transferId);

            if(response.Success)
                return Ok(response.Data);

            return BadRequest(response.Message);
        }
    }
}
