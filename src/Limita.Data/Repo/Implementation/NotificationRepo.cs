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

    public async Task<Notification?> GetByIdAndUserIdAsync(int notificationId, int userId)
    {
        return await dbContext.Notifications
            .AsNoTracking()
            .FirstOrDefaultAsync(notification =>
                notification.Id == notificationId &&
                notification.UserId == userId);
    }

    public async Task<bool> MarkAsReadAsync(int notificationId, int userId)
    {
        Notification? notification = await dbContext.Notifications
            .FirstOrDefaultAsync(notification =>
                notification.Id == notificationId &&
                notification.UserId == userId);

        if (notification is null)
            return false;

        if (notification.IsRead)
            return true;

        notification.IsRead = true;

        await dbContext.SaveChangesAsync();

        return true;
    }
}
