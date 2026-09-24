namespace Limita.Data.Repo.Implementation;

public class BeneficiaryRepo : IBeneficiaryRepo
{
    private readonly LimitaDbContext dbContext;

    public BeneficiaryRepo(LimitaDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<int> AddBeneficiaryAsync(Beneficiary beneficiary)
    {
        await dbContext.Beneficiaries.AddAsync(beneficiary);
        await dbContext.SaveChangesAsync();

        return beneficiary.Id;
    }

    public async Task<bool> DeleteBeneficiaryAsync(int beneficiaryId)
    {
        Beneficiary? beneficiary = await GetBeneficiaryAsync(beneficiaryId);

        if (beneficiary is null)
            return false;

        dbContext.Beneficiaries.Remove(beneficiary);
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExistingByUseridAndIdentifierAsync(
        int userId,
        string accountIdentifier)
    {
        return await dbContext.Beneficiaries.AnyAsync(beneficiary =>
            beneficiary.UserId == userId &&
            beneficiary.AccountIdentifier == accountIdentifier);
    }

    public async Task<List<Beneficiary>> GetBeneficiariesAsync(int userId)
    {
        return await dbContext.Beneficiaries
            .AsNoTracking()
            .Where(beneficiary => beneficiary.UserId == userId)
            .ToListAsync();
    }

    public async Task<Beneficiary?> GetBeneficiaryAsync(int beneficiaryId)
    {
        return await dbContext.Beneficiaries
            .FirstOrDefaultAsync(beneficiary => beneficiary.Id == beneficiaryId);
    }

    public async Task<bool> UpdateBeneficiaryAsync(Beneficiary beneficiary)
    {
        dbContext.Beneficiaries.Update(beneficiary);
        await dbContext.SaveChangesAsync();

        return true;
    }
}
