using Limita.Business.Common;
using Limita.Business.DTOs.Beneficiary;
using Limita.Business.DTOs.Transfer;
using Limita.Business.Services.Interface;
using Limita.Data;
using Limita.Data.Entities;
using Limita.Data.Repo.Interface;

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
        IBeneficiaryService beneficiaryService,ITransactionRepo transactionRepo)
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
        if (requestDTO.Amount <= 0)
        {
            return new ServiceResult<TransferResponseDTO>
            {
                Success = false,
                Message = "Amount must be greater than zero"
            };
        }

        var account = await accountRepo.GetByIdAndUserIdAsync(
            requestDTO.SourceAccountId,
            userId);

        if (account is null)
        {
            return new ServiceResult<TransferResponseDTO>
            {
                Success = false,
                Message = "Source account not found"
            };
        }

        if (account.Balance < requestDTO.Amount)
        {
            return new ServiceResult<TransferResponseDTO>
            {
                Success = false,
                Message = "Insufficient balance"
            };
        }

        var beneficiaryResult = await beneficiaryService.GetBeneficiaryById(
            userId,
            requestDTO.BeneficiaryId);

        if (!beneficiaryResult.Success || beneficiaryResult.Data is null)
        {
            return new ServiceResult<TransferResponseDTO>
            {
                Success = false,
                Message = "Beneficiary not found"
            };
        }

        await using var dbTransaction =
            await dbContext.Database.BeginTransactionAsync();

        try
        {
            account.Balance -= requestDTO.Amount;

            var transaction = new Transaction
            {
                UserId = userId,
                AccountId = account.Id,
                BeneficiaryId = requestDTO.BeneficiaryId,
                Amount = requestDTO.Amount,
                Currency = account.Currency,
                Type = Data.Entities.Enums.TransactionType.Transfer,
                Status = Data.Entities.Enums.TransactionStatus.Completed,
                Reference = Guid.NewGuid().ToString("N"),
                Note = requestDTO.Note,
                CreatedAt = DateTime.UtcNow
            };

            await  transactionRepo.AddTransactionAsync(transaction);

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
        catch
        {
            await dbTransaction.RollbackAsync();

            return new ServiceResult<TransferResponseDTO>
            {
                Success = false,
                Message = "Transfer failed"
            };
        }
    }

    public async Task<ServiceResult<TransferResponseDTO>> GetTransferByIdAsync(int userId, int transactionId)
    {
        var transfer = await transactionRepo.GetTransferAsync(transactionId);

        if(transfer == null || transfer.UserId != userId || transfer.BeneficiaryId is null)
            return new ServiceResult<TransferResponseDTO> { Success = false, Message = "Invalid operation" };


         var beneficiary = await beneficiaryService.GetBeneficiaryById(userId, (int)transfer.BeneficiaryId );

        if(beneficiary.Data is  null || !beneficiary.Success)
              return new ServiceResult<TransferResponseDTO> { Success = false, Message = "Invalid operation" };


        var transferresponse = new TransferResponseDTO
        { 
        
             Amount = transfer.Amount,
              BeneficiaryName = beneficiary.Data.Name,
               Currency = transfer.Currency,
                Date = transfer.CreatedAt,
                 Status = transfer.Status,
                  TransactionReference= transfer.Reference


        };

        return new ServiceResult<TransferResponseDTO> { Success = true, Message = "transfer retrieved" , Data = transferresponse };

    }
}
