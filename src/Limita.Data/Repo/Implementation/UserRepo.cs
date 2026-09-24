namespace Limita.Data.Repo.Implementation;

public class UserRepo : IUserRepo
{
    private readonly LimitaDbContext dbContext;

    public UserRepo(LimitaDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<bool> ExistsByEmailOrPhoneAsync(string email, string phone)
    {
        return await dbContext.Users
            .AnyAsync(user =>
                user.Email == email ||
                user.PhoneNumber == phone);
    }

    public async Task<int> AddUserAsync(User user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return user.Id;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(user => user.Id == userId);
    }

    public async Task<bool> ExistsByEmailOrPhoneForOtherUserAsync(
        int userId,
        string email,
        string phone)
    {
        return await dbContext.Users
            .AnyAsync(user =>
                user.Id != userId &&
                (user.Email == email || user.PhoneNumber == phone));
    }

    public async Task UpdateAsync(User user)
    {
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
    }
}
