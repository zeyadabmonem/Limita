namespace Limita.API.Controllers;

[Authorize]
[Route("api/v1/beneficiaries")]
[ApiController]
public class BeneficiaryController : ControllerBase
{
    private readonly IBeneficiaryService beneficiaryService;

    public BeneficiaryController(IBeneficiaryService beneficiaryService)
    {
        this.beneficiaryService = beneficiaryService;
    }

    [HttpPost]
    public async Task<IActionResult> AddBeneficiary(
        [FromBody] AddBeneficiaryRequestDTO requestDTO)
    {
        ServiceResult<BeneficiaryResponseDTO> result =
            await beneficiaryService.AddBeneficiary(User.GetUserId(), requestDTO);

        return result.ToActionResult(this);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        ServiceResult<List<BeneficiaryResponseDTO>> result =
            await beneficiaryService.GetAllBeneficiaries(User.GetUserId());

        return result.ToActionResult(this);
    }

    [HttpGet("{beneficiaryId:int}")]
    public async Task<IActionResult> GetById(int beneficiaryId)
    {
        ServiceResult<BeneficiaryResponseDTO> result =
            await beneficiaryService.GetBeneficiaryById(
                User.GetUserId(),
                beneficiaryId);

        return result.ToActionResult(this);
    }

    [HttpPut("{beneficiaryId:int}")]
    public async Task<IActionResult> UpdateBeneficiary(
        int beneficiaryId,
        [FromBody] UpdateBeneficiaryRequestDTO requestDTO)
    {
        ServiceResult<BeneficiaryResponseDTO> result =
            await beneficiaryService.UpdateBeneficiary(
                User.GetUserId(),
                beneficiaryId,
                requestDTO);

        return result.ToActionResult(this);
    }

    [HttpDelete("{beneficiaryId:int}")]
    public async Task<IActionResult> DeleteBeneficiary(int beneficiaryId)
    {
        ServiceResult<bool> result =
            await beneficiaryService.DeleteBeneficiary(
                User.GetUserId(),
                beneficiaryId);

        if (!result.Success)
            return result.ToActionResult(this);

        return NoContent();
    }
}
