namespace Limita.Data.Repo.Implementation;

public class NotificationRepo : INotificationRepo
{
    private readonly LimitaDbContext dbContext;

    public NotificationRepo(LimitaDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<Notification>> GetByUserIdAsync(int userId)
    {
        return await dbContext.Notifications
            .AsNoTracking()
            .Where(notification => notification.UserId == userId)
            .OrderByDescending(notification => notification.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> MarkAsReadAsync(int notificationId, int userId)
    {
        int affectedRows = await dbContext.Notifications
            .Where(notification =>
                notification.Id == notificationId &&
                notification.UserId == userId)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(notification => notification.IsRead, true));

        return affectedRows > 0;
    }
}
