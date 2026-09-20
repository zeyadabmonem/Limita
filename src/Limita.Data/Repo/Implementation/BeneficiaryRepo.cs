using Limita.Data.Entities;
using Limita.Data.Repo.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Data.Repo.Implementation
{
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
            var b = await GetBeneficiaryAsync(beneficiaryId);
            if (b != null)
            {
                dbContext.Beneficiaries.Remove(b);
                await dbContext.SaveChangesAsync();
                return true;
            }

            return false;

        }

        public async Task<bool> ExistingByUseridAndIdentifierAsync(int userId, string accountIdentifier)
        {
            if (await dbContext.Beneficiaries.AnyAsync(b => b.UserId == userId && b.AccountIdentifier == accountIdentifier))
                return true;
            return false;
        }

        public async Task<List<Beneficiary>> GetBeneficiariesAsync(int userId)
        {
            return await dbContext.Beneficiaries.Where(b => b.UserId == userId).ToListAsync();

        }

        public async Task<Beneficiary?> GetBeneficiaryAsync(int beneficiaryId)
        {
            var beneficiary = await dbContext.Beneficiaries.FirstOrDefaultAsync(b => b.Id == beneficiaryId);
            return beneficiary == null ? null : beneficiary;
        }

        public async Task<bool> UpdateBeneficiaryAsync(Beneficiary beneficiary)
        {
            if (beneficiary == null)
                return false;

            dbContext.Beneficiaries.Update(beneficiary);
            await dbContext.SaveChangesAsync();
            return true;
        }
        
    }
}
