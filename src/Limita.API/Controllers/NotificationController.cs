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
    public async Task<IActionResult> GetNotifications()
    {
        ServiceResult<List<NotificationResponseDTO>> result =
            await notificationService.GetNotificationsAsync(User.GetUserId());

        return result.ToActionResult(this);
    }

    [HttpPatch("{notificationId:int}/read")]
    public async Task<IActionResult> MarkAsRead(int notificationId)
    {
        ServiceResult<bool> result =
            await notificationService.MarkAsReadAsync(
                User.GetUserId(),
                notificationId);

        if (!result.Success)
            return result.ToActionResult(this);

        return NoContent();
    }
}
