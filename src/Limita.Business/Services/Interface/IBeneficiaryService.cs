namespace Limita.Business.Services.Interface
{
    public interface IBeneficiaryService
    {
        
        Task<ServiceResult<BeneficiaryResponseDTO>> AddBeneficiary(int userId,AddBeneficiaryRequestDTO requestDTO);
        Task<ServiceResult<List<BeneficiaryResponseDTO>>> GetAllBeneficiaries(int userId);
        Task<ServiceResult<BeneficiaryResponseDTO>> GetBeneficiaryById(int userId , int beneficiaryId);
        Task<ServiceResult<BeneficiaryResponseDTO>> UpdateBeneficiary(int userId, int beneficiaryId, UpdateBeneficiaryRequestDTO requestDTO);
        Task<ServiceResult<bool>> DeleteBeneficiary(int userId, int beneficiaryId);
    }
}
