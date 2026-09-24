namespace Limita.Business.Services.Implementation;

public class NotificationService : INotificationService
{
    private readonly INotificationRepo notificationRepo;

    public NotificationService(INotificationRepo notificationRepo)
    {
        this.notificationRepo = notificationRepo;
    }

    public async Task<ServiceResult<List<NotificationResponseDTO>>> GetNotificationsAsync(int userId)
    {
        List<Notification> notifications = await notificationRepo.GetByUserIdAsync(userId);

        return new ServiceResult<List<NotificationResponseDTO>>
        {
            Success = true,
            Message = "Notifications retrieved successfully",
            Data = notifications.Select(MapToResponse).ToList()
        };
    }

    public async Task<ServiceResult<bool>> MarkAsReadAsync(int userId, int notificationId)
    {
        if (notificationId <= 0)
        {
            return new ServiceResult<bool>
            {
                Success = false,
                Message = "Notification id must be greater than zero"
            };
        }

        bool markedAsRead = await notificationRepo.MarkAsReadAsync(notificationId, userId);

        if (!markedAsRead)
        {
            return new ServiceResult<bool>
            {
                Success = false,
                Message = "Notification not found"
            };
        }

        return new ServiceResult<bool>
        {
            Success = true,
            Message = "Notification marked as read",
            Data = true
        };
    }

    private static NotificationResponseDTO MapToResponse(Notification notification)
    {
        return new NotificationResponseDTO
        {
            Id = notification.Id,
            Title = notification.Title,
            Message = notification.Message,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt
        };
    }
}
