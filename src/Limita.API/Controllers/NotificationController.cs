namespace Limita.API.Controllers;

[Authorize]
[Route("api/v1/notifications")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationService notificationService;

    public NotificationController(INotificationService notificationService)
    {
        this.notificationService = notificationService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<NotificationResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<NotificationResponseDTO>>> GetNotifications()
    {
        var response = await notificationService.GetNotificationsAsync(User.GetUserId());

        if (!response.Success)
            return BadRequest(response.Message);

        return Ok(response.Data);
    }

    [HttpPatch("{notificationId:int}/read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> MarkAsRead(int notificationId)
    {
        var response = await notificationService.MarkAsReadAsync(
            User.GetUserId(),
            notificationId);

        if (!response.Success)
            return BadRequest(response.Message);

        return NoContent();
    }
}
