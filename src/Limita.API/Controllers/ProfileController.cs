namespace Limita.API.Controllers;

[Authorize]
[Route("api/v1/profile")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IProfileService profileService;

    public ProfileController(IProfileService profileService)
    {
        this.profileService = profileService;
    }

    [HttpGet]
    public async Task<ActionResult<ProfileResponseDTO>> GetProfile()
    {
        int? userId = GetAuthenticatedUserId();
        if (userId == null)
            return Unauthorized();

        ServiceResult<ProfileResponseDTO> result = await profileService.GetProfileAsync(userId.Value);
        if (!result.Success)
            return NotFound(result.Message);

        return Ok(result.Data);
    }

    [HttpPut]
    public async Task<ActionResult<ProfileResponseDTO>> UpdateProfile([FromBody] UpdateProfileRequestDTO request)
    {
        int? userId = GetAuthenticatedUserId();
        if (userId == null)
            return Unauthorized();

        ServiceResult<ProfileResponseDTO> result = await profileService.UpdateProfileAsync(userId.Value, request);
        if (!result.Success)
            return result.Message == "User not found" ? NotFound(result.Message) : BadRequest(result.Message);

        return Ok(result.Data);
    }

    private int? GetAuthenticatedUserId()
    {
        string? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out int userId) ? userId : null;
    }
}
