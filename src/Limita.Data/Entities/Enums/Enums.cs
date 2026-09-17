namespace Limita.Data.Entities.Enums;

public enum AccountStatus
{
    Active = 0,
    Frozen = 1,
    Closed = 2
}

public enum CardStatus
{
    Active = 0,
    Frozen = 1,
    Expired = 2
}

public enum TransactionType
{
    Transfer = 0,
    BillPayment = 1,
    Deposit = 2
}

public enum TransactionStatus
{
    Pending = 0,
    Completed = 1,
    Failed = 2
}

public enum BillStatus
{
    Unpaid = 0,
    Paid = 1,
    Overdue = 2
}
