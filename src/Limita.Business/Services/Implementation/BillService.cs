using Limita.Business.Common;
using Limita.Business.DTOs.Bill;
using Limita.Business.DTOs.Bills;
using Limita.Business.Services.Interface;
using Limita.Data;
using Limita.Data.Entities;
using Limita.Data.Entities.Enums;
using Limita.Data.Repo.Interface;

namespace Limita.Business.Services.Implementation;

public class BillService : IBillService
{
    private readonly LimitaDbContext dbContext;
    private readonly IBillRepo billRepo;
    private readonly IAccountRepo accountRepo;
    private readonly ITransactionRepo transactionRepo;

    public BillService(
        LimitaDbContext dbContext,
        IBillRepo billRepo,
        IAccountRepo accountRepo,
        ITransactionRepo transactionRepo)
    {
        this.dbContext = dbContext;
        this.billRepo = billRepo;
        this.accountRepo = accountRepo;
        this.transactionRepo = transactionRepo;
    }

    public async Task<ServiceResult<List<BillResponseDTO>>> GetBillsAsync(int userId)
    {
        List<Bill> bills = await billRepo.GetByUserIdAsync(userId);

        return new ServiceResult<List<BillResponseDTO>>
        {
            Success = true,
            Message = "Bills retrieved successfully",
            Data = bills.Select(MapToResponse).ToList()
        };
    }

    public async Task<ServiceResult<BillResponseDTO>> GetBillByIdAsync(int userId, int billId)
    {
        Bill? bill = await billRepo.GetByIdAndUserIdAsync(billId, userId);

        if (bill is null)
        {
            return new ServiceResult<BillResponseDTO>
            {
                Success = false,
                Message = "Bill not found"
            };
        }

        return new ServiceResult<BillResponseDTO>
        {
            Success = true,
            Message = "Bill retrieved successfully",
            Data = MapToResponse(bill)
        };
    }

    public async Task<ServiceResult<PayBillResponseDTO>> PayBillAsync(
        int userId,
        int billId,
        PayBillRequestDTO requestDTO)
    {
        Bill? bill = await billRepo.GetByIdAndUserIdAsync(billId, userId);

        if (bill is null)
        {
            return new ServiceResult<PayBillResponseDTO>
            {
                Success = false,
                Message = "Bill not found"
            };
        }

        if (bill.Amount <= 0)
        {
            return new ServiceResult<PayBillResponseDTO>
            {
                Success = false,
                Message = "Bill amount must be greater than zero"
            };
        }

        if (bill.Status == BillStatus.Paid)
        {
            return new ServiceResult<PayBillResponseDTO>
            {
                Success = false,
                Message = "Bill is already paid"
            };
        }

        Account? account = await accountRepo.GetByIdAndUserIdAsync(requestDTO.SourceAccountId, userId);

        if (account is null)
        {
            return new ServiceResult<PayBillResponseDTO>
            {
                Success = false,
                Message = "Source account not found"
            };
        }

        if (account.Balance < bill.Amount)
        {
            return new ServiceResult<PayBillResponseDTO>
            {
                Success = false,
                Message = "Insufficient balance"
            };
        }

        await using var dbTransaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            account.Balance -= bill.Amount;
            bill.Status = BillStatus.Paid;
            bill.PaidAt = DateTime.UtcNow;

            var transaction = new Transaction
            {
                UserId = userId,
                AccountId = account.Id,
                BillId = bill.Id,
                Type = TransactionType.BillPayment,
                Amount = bill.Amount,
                Currency = account.Currency,
                Status = TransactionStatus.Completed,
                Reference = Guid.NewGuid().ToString("N"),
                Note = $"Payment for {bill.ProviderName} bill {bill.BillNumber}",
                CreatedAt = DateTime.UtcNow
            };

            await transactionRepo.AddTransactionAsync(transaction);

            await dbTransaction.CommitAsync();

            return new ServiceResult<PayBillResponseDTO>
            {
                Success = true,
                Message = "Bill paid successfully",
                Data = new PayBillResponseDTO
                {
                    BillId = bill.Id,
                    ProviderName = bill.ProviderName,
                    Amount = transaction.Amount,
                    Currency = transaction.Currency,
                    Status = bill.Status.ToString(),
                    PaidAt = bill.PaidAt.Value,
                    TransactionReference = transaction.Reference
                }
            };
        }
        catch
        {
            await dbTransaction.RollbackAsync();

            return new ServiceResult<PayBillResponseDTO>
            {
                Success = false,
                Message = "Bill payment failed"
            };
        }
    }

    private static BillResponseDTO MapToResponse(Bill bill)
    {
        return new BillResponseDTO
        {
            Id = bill.Id,
            ProviderName = bill.ProviderName,
            BillNumber = bill.BillNumber,
            Amount = bill.Amount,
            DueDate = bill.DueDate,
            Status = bill.Status.ToString(),
            PaidAt = bill.PaidAt
        };
    }
}