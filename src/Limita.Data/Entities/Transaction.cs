namespace Limita.Data.Entities;

public class Transaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AccountId { get; set; }
    public int? BeneficiaryId { get; set; }
    public int? BillId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public string Reference { get; set; } = string.Empty; // unique, shown on receipt
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
    public Account Account { get; set; } = null!;
    public Beneficiary? Beneficiary { get; set; }
    public Bill? Bill { get; set; }
}
