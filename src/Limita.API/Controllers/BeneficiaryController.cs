namespace Limita.API.Controllers
{
    [Authorize]
    [Route("api/v1/beneficiaries")]
    [ApiController]
    public class BeneficiaryController : ControllerBase
    {
        private readonly IBeneficiaryService service;

        public BeneficiaryController(IBeneficiaryService service) 
        {
            
            this.service = service;
        }
        [HttpPost]
        public async Task<ActionResult> AddBeneficiary([FromBody] AddBeneficiaryRequestDTO requestDTO)
        {
            ServiceResult<BeneficiaryResponseDTO> responseDTO = await service.AddBeneficiary(User.GetUserId(),requestDTO);
            if(responseDTO.Success)
              return  Ok(responseDTO.Data);

            return BadRequest(responseDTO.Message);

        }
        [HttpGet]
        public async Task<ActionResult>GetAll()
        {
            ServiceResult<List<BeneficiaryResponseDTO>> responseDTO = await service.GetAllBeneficiaries(User.GetUserId());
            if(responseDTO.Success)
              return  Ok(responseDTO.Data);

            return BadRequest(responseDTO.Message);

        }

        [HttpGet("{beneficiaryId}")]
        public async Task<ActionResult> GetById(int beneficiaryId)
        {
            ServiceResult<BeneficiaryResponseDTO> responseDTO = await service.GetBeneficiaryById(User.GetUserId(),beneficiaryId);
            if(responseDTO.Success)
              return  Ok(responseDTO.Data);

            return BadRequest(responseDTO.Message);

        }

        [HttpPut("{beneficiaryId}")]
        public async Task<ActionResult> UpdateBeneficiary( int beneficiaryId, [FromBody] UpdateBeneficiaryRequestDTO requestDTO)
        {
            ServiceResult<BeneficiaryResponseDTO> responseDTO = await service.UpdateBeneficiary(User.GetUserId(), beneficiaryId, requestDTO);
            if(responseDTO.Success)
              return  Ok(responseDTO.Data);

            return BadRequest(responseDTO.Message);

        }

        [HttpDelete("{beneficiaryId}")]
        public async Task<ActionResult> DeleteBeneficiary( int beneficiaryId)
        {
            ServiceResult<bool> responseDTO = await service.DeleteBeneficiary(User.GetUserId(),beneficiaryId);
            if(responseDTO.Success)
              return  Ok(responseDTO.Message);

            return BadRequest(responseDTO.Message);

        }


    }
}
