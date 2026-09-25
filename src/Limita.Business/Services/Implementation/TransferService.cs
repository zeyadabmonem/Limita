using Limita.Data.Entities.Enums;

namespace Limita.Business.Services.Implementation;

public class TransferService : ITransferService
{
    private readonly LimitaDbContext dbContext;
    private readonly IAccountRepo accountRepo;
    private readonly IBeneficiaryService beneficiaryService;
    private readonly ITransactionRepo transactionRepo;

    public TransferService(
        LimitaDbContext dbContext,
        IAccountRepo accountRepo,
        IBeneficiaryService beneficiaryService,
        ITransactionRepo transactionRepo)
    {
        this.dbContext = dbContext;
        this.accountRepo = accountRepo;
        this.beneficiaryService = beneficiaryService;
        this.transactionRepo = transactionRepo;
    }

    public async Task<ServiceResult<TransferResponseDTO>> AddNewTransferAsync(
        int userId,
        TransferRequestDTO requestDTO)
    {
        if (requestDTO.SourceAccountId <= 0 ||
            requestDTO.BeneficiaryId <= 0 ||
            requestDTO.Amount <= 0)
        {
            return Failure<TransferResponseDTO>(
                "Source account, beneficiary and amount must be valid",
                ServiceErrorCode.Validation);
        }

        Account? account = await accountRepo.GetByIdAndUserIdAsync(
            requestDTO.SourceAccountId,
            userId);

        if (account is null)
            return Failure<TransferResponseDTO>("Source account not found", ServiceErrorCode.NotFound);

        if (account.Balance < requestDTO.Amount)
        {
            return Failure<TransferResponseDTO>(
                "Insufficient balance",
                ServiceErrorCode.UnprocessableEntity);
        }

        ServiceResult<BeneficiaryResponseDTO> beneficiaryResult =
            await beneficiaryService.GetBeneficiaryById(
                userId,
                requestDTO.BeneficiaryId);

        if (!beneficiaryResult.Success || beneficiaryResult.Data is null)
        {
            return Failure<TransferResponseDTO>(
                "Beneficiary not found",
                ServiceErrorCode.NotFound);
        }

        await using var dbTransaction =
            await dbContext.Database.BeginTransactionAsync();

        try
        {
            DateTime createdAt = DateTime.UtcNow;
            string transactionReference = Guid.NewGuid().ToString("N");

            account.Balance -= requestDTO.Amount;

            Transaction transaction = new()
            {
                UserId = userId,
                AccountId = account.Id,
                BeneficiaryId = requestDTO.BeneficiaryId,
                Amount = requestDTO.Amount,
                Currency = account.Currency,
                Type = TransactionType.Transfer,
                Status = TransactionStatus.Completed,
                Reference = transactionReference,
                Note = requestDTO.Note,
                CreatedAt = createdAt
            };

            await transactionRepo.AddTransactionAsync(transaction);
            await dbTransaction.CommitAsync();

            return new ServiceResult<TransferResponseDTO>
            {
                Success = true,
                Message = "Transfer completed successfully",
                Data = new TransferResponseDTO
                {
                    Amount = transaction.Amount,
                    BeneficiaryName = beneficiaryResult.Data.Name,
                    Currency = transaction.Currency,
                    Date = transaction.CreatedAt,
                    Status = transaction.Status,
                    TransactionReference = transaction.Reference
                }
            };
        }
        catch (Exception)
        {
            await dbTransaction.RollbackAsync();

            return Failure<TransferResponseDTO>(
                "Transfer failed",
                ServiceErrorCode.Unexpected);
        }
    }

    public async Task<ServiceResult<TransferResponseDTO>> GetTransferByIdAsync(
        int userId,
        int transactionId)
    {
        if (transactionId <= 0)
            return Failure<TransferResponseDTO>(
                "Transfer id must be greater than zero",
                ServiceErrorCode.Validation);

        Transaction? transfer = await transactionRepo.GetTransferAsync(transactionId);

        if (transfer is null ||
            transfer.UserId != userId ||
            transfer.BeneficiaryId is null)
        {
            return Failure<TransferResponseDTO>(
                "Transfer not found",
                ServiceErrorCode.NotFound);
        }

        ServiceResult<BeneficiaryResponseDTO> beneficiary =
            await beneficiaryService.GetBeneficiaryById(
                userId,
                transfer.BeneficiaryId.Value);

        if (!beneficiary.Success || beneficiary.Data is null)
        {
            return Failure<TransferResponseDTO>(
                "Transfer not found",
                ServiceErrorCode.NotFound);
        }

        return new ServiceResult<TransferResponseDTO>
        {
            Success = true,
            Message = "Transfer retrieved successfully",
            Data = new TransferResponseDTO
            {
                Amount = transfer.Amount,
                BeneficiaryName = beneficiary.Data.Name,
                Currency = transfer.Currency,
                Date = transfer.CreatedAt,
                Status = transfer.Status,
                TransactionReference = transfer.Reference
            }
        };
    }

    private static ServiceResult<T> Failure<T>(string message, ServiceErrorCode errorCode) =>
        new()
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode
        };
}
