namespace Limita.Data.Repo.Interface
{
    public interface IBeneficiaryRepo
    {
        Task<int> AddBeneficiaryAsync(Beneficiary beneficiary);
        Task<List<Beneficiary>> GetBeneficiariesAsync(int userId);
        Task<Beneficiary?> GetBeneficiaryAsync(int beneficiaryId);
        Task<bool> UpdateBeneficiaryAsync(Beneficiary beneficiary);
        Task<bool> DeleteBeneficiaryAsync(int beneficiaryId);

        Task<bool> ExistingByUseridAndIdentifierAsync(int userId,string accountIdentifier);
    }
}