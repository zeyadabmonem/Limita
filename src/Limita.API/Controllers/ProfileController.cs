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
        ServiceResult<ProfileResponseDTO> result =
            await profileService.GetProfileAsync(User.GetUserId());

        return result.ToActionResult(this);
    }

    [HttpPut]
    public async Task<ActionResult<ProfileResponseDTO>> UpdateProfile(
        [FromBody] UpdateProfileRequestDTO request)
    {
        ServiceResult<ProfileResponseDTO> result =
            await profileService.UpdateProfileAsync(
                User.GetUserId(),
                request);

        return result.ToActionResult(this);
    }
}
