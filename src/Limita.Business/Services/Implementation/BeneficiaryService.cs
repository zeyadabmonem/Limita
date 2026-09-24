namespace Limita.Business.Services.Implementation;

public class BeneficiaryService : IBeneficiaryService
{
    private readonly IBeneficiaryRepo beneficiaryRepo;

    public BeneficiaryService(IBeneficiaryRepo beneficiaryRepo)
    {
        this.beneficiaryRepo = beneficiaryRepo;
    }

    public async Task<ServiceResult<BeneficiaryResponseDTO>> AddBeneficiary(
        int userId,
        AddBeneficiaryRequestDTO requestDTO)
    {
        if (string.IsNullOrWhiteSpace(requestDTO.Name) ||
            string.IsNullOrWhiteSpace(requestDTO.AccountIdentifier))
        {
            return Failure<BeneficiaryResponseDTO>(
                "Name and account identifier are required",
                ServiceErrorCode.Validation);
        }

        if (await beneficiaryRepo.ExistingByUseridAndIdentifierAsync(
                userId,
                requestDTO.AccountIdentifier))
        {
            return Failure<BeneficiaryResponseDTO>(
                "Beneficiary already exists",
                ServiceErrorCode.Conflict);
        }

        Beneficiary beneficiary = new()
        {
            AccountIdentifier = requestDTO.AccountIdentifier,
            CreatedAt = DateTime.UtcNow,
            Name = requestDTO.Name,
            UserId = userId
        };

        int id = await beneficiaryRepo.AddBeneficiaryAsync(beneficiary);

        return new ServiceResult<BeneficiaryResponseDTO>
        {
            Success = true,
            Message = "Beneficiary created successfully",
            Data = new BeneficiaryResponseDTO
            {
                AccountIdentifier = beneficiary.AccountIdentifier,
                Id = id,
                Name = beneficiary.Name
            }
        };
    }

    public async Task<ServiceResult<bool>> DeleteBeneficiary(int userId, int beneficiaryId)
    {
        if (beneficiaryId <= 0)
            return Failure<bool>("Beneficiary id must be greater than zero", ServiceErrorCode.Validation);

        Beneficiary? beneficiary = await beneficiaryRepo.GetBeneficiaryAsync(beneficiaryId);

        if (beneficiary is null || beneficiary.UserId != userId)
            return Failure<bool>("Beneficiary not found", ServiceErrorCode.NotFound);

        await beneficiaryRepo.DeleteBeneficiaryAsync(beneficiaryId);

        return new ServiceResult<bool>
        {
            Success = true,
            Message = "Beneficiary deleted successfully",
            Data = true
        };
    }

    public async Task<ServiceResult<List<BeneficiaryResponseDTO>>> GetAllBeneficiaries(int userId)
    {
        List<Beneficiary> beneficiaries = await beneficiaryRepo.GetBeneficiariesAsync(userId);

        return new ServiceResult<List<BeneficiaryResponseDTO>>
        {
            Success = true,
            Message = "Beneficiaries retrieved successfully",
            Data = beneficiaries.Select(MapToResponse).ToList()
        };
    }

    public async Task<ServiceResult<BeneficiaryResponseDTO>> GetBeneficiaryById(
        int userId,
        int beneficiaryId)
    {
        if (beneficiaryId <= 0)
            return Failure<BeneficiaryResponseDTO>(
                "Beneficiary id must be greater than zero",
                ServiceErrorCode.Validation);

        Beneficiary? beneficiary = await beneficiaryRepo.GetBeneficiaryAsync(beneficiaryId);

        if (beneficiary is null || beneficiary.UserId != userId)
            return Failure<BeneficiaryResponseDTO>("Beneficiary not found", ServiceErrorCode.NotFound);

        return new ServiceResult<BeneficiaryResponseDTO>
        {
            Success = true,
            Message = "Beneficiary retrieved successfully",
            Data = MapToResponse(beneficiary)
        };
    }

    public async Task<ServiceResult<BeneficiaryResponseDTO>> UpdateBeneficiary(
        int userId,
        int beneficiaryId,
        UpdateBeneficiaryRequestDTO requestDTO)
    {
        if (beneficiaryId <= 0)
            return Failure<BeneficiaryResponseDTO>(
                "Beneficiary id must be greater than zero",
                ServiceErrorCode.Validation);

        if (string.IsNullOrWhiteSpace(requestDTO.Name) ||
            string.IsNullOrWhiteSpace(requestDTO.AccountIdentifier))
        {
            return Failure<BeneficiaryResponseDTO>(
                "Name and account identifier are required",
                ServiceErrorCode.Validation);
        }

        Beneficiary? beneficiary = await beneficiaryRepo.GetBeneficiaryAsync(beneficiaryId);

        if (beneficiary is null || beneficiary.UserId != userId)
            return Failure<BeneficiaryResponseDTO>("Beneficiary not found", ServiceErrorCode.NotFound);

        beneficiary.AccountIdentifier = requestDTO.AccountIdentifier;
        beneficiary.Name = requestDTO.Name;

        await beneficiaryRepo.UpdateBeneficiaryAsync(beneficiary);

        return new ServiceResult<BeneficiaryResponseDTO>
        {
            Success = true,
            Message = "Beneficiary updated successfully",
            Data = MapToResponse(beneficiary)
        };
    }

    private static ServiceResult<T> Failure<T>(string message, ServiceErrorCode errorCode) =>
        new()
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode
        };

    private static BeneficiaryResponseDTO MapToResponse(Beneficiary beneficiary) =>
        new()
        {
            Id = beneficiary.Id,
            AccountIdentifier = beneficiary.AccountIdentifier,
            Name = beneficiary.Name
        };
}
