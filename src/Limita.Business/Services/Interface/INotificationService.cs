namespace Limita.Business.Services.Interface;

public interface INotificationService
{
    Task<ServiceResult<List<NotificationResponseDTO>>> GetNotificationsAsync(int userId);
    Task<ServiceResult<bool>> MarkAsReadAsync(int userId, int notificationId);
}
