using Limita.Data.Entities.Enums;

namespace Limita.Business.Services.Implementation;

public class BillService : IBillService
{
    private readonly LimitaDbContext dbContext;
    private readonly IBillRepo billRepo;
    private readonly IAccountRepo accountRepo;
    private readonly ITransactionRepo transactionRepo;
    private readonly INotificationRepo notificationRepo;

    public BillService(
        LimitaDbContext dbContext,
        IBillRepo billRepo,
        IAccountRepo accountRepo,
        ITransactionRepo transactionRepo,
        INotificationRepo notificationRepo)
    {
        this.dbContext = dbContext;
        this.billRepo = billRepo;
        this.accountRepo = accountRepo;
        this.transactionRepo = transactionRepo;
        this.notificationRepo = notificationRepo;
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
        if (billId <= 0)
            return Failure<BillResponseDTO>("Bill id must be greater than zero", ServiceErrorCode.Validation);

        Bill? bill = await billRepo.GetByIdAndUserIdAsync(billId, userId);

        if (bill is null)
            return Failure<BillResponseDTO>("Bill not found", ServiceErrorCode.NotFound);

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
        if (billId <= 0 || requestDTO.SourceAccountId <= 0)
            return Failure<PayBillResponseDTO>(
                "Bill id and source account id must be greater than zero",
                ServiceErrorCode.Validation);

        Bill? bill = await billRepo.GetByIdAndUserIdAsync(billId, userId);

        if (bill is null)
            return Failure<PayBillResponseDTO>("Bill not found", ServiceErrorCode.NotFound);

        if (bill.Amount <= 0)
            return Failure<PayBillResponseDTO>("Bill amount must be greater than zero", ServiceErrorCode.Validation);

        if (bill.Status == BillStatus.Paid)
            return Failure<PayBillResponseDTO>("Bill is already paid", ServiceErrorCode.Conflict);

        Account? account = await accountRepo.GetByIdAndUserIdAsync(
            requestDTO.SourceAccountId,
            userId);

        if (account is null)
            return Failure<PayBillResponseDTO>("Source account not found", ServiceErrorCode.NotFound);

        if (account.Balance < bill.Amount)
            return Failure<PayBillResponseDTO>("Insufficient balance", ServiceErrorCode.UnprocessableEntity);

        await using var dbTransaction =
            await dbContext.Database.BeginTransactionAsync();

        try
        {
            DateTime paidAt = DateTime.UtcNow;
            string transactionReference = Guid.NewGuid().ToString("N");

            account.Balance -= bill.Amount;
            bill.Status = BillStatus.Paid;
            bill.PaidAt = paidAt;

            Transaction transaction = new()
            {
                UserId = userId,
                AccountId = account.Id,
                BillId = bill.Id,
                Type = TransactionType.BillPayment,
                Amount = bill.Amount,
                Currency = account.Currency,
                Status = TransactionStatus.Completed,
                Reference = transactionReference,
                Note = $"Payment for {bill.ProviderName} bill {bill.BillNumber}",
                CreatedAt = paidAt
            };

            await transactionRepo.AddTransactionAsync(transaction);

            Notification notification = new()
            {
                UserId = userId,
                Title = "Bill payment successful",
                Message = $"{bill.ProviderName} bill {bill.BillNumber} was paid successfully.",
                IsRead = false,
                CreatedAt = paidAt
            };

            await notificationRepo.AddAsync(notification);
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
                    PaidAt = paidAt,
                    TransactionReference = transactionReference
                }
            };
        }
        catch (Exception)
        {
            await dbTransaction.RollbackAsync();

            return Failure<PayBillResponseDTO>(
                "Bill payment failed",
                ServiceErrorCode.Unexpected);
        }
    }

    private static ServiceResult<T> Failure<T>(
        string message,
        ServiceErrorCode errorCode)
    {
        return new ServiceResult<T>
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode
        };
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
