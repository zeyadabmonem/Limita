namespace Limita.Data.Repo.Interface;

public interface INotificationRepo
{
    Task<List<Notification>> GetByUserIdAsync(int userId);
    Task<Notification?> GetByIdAndUserIdAsync(int notificationId, int userId);
    Task<bool> MarkAsReadAsync(int notificationId, int userId);
}
