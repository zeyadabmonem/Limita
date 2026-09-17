using Limita.Data.Entities.Enums;

namespace Limita.Data.Entities;

public class Bill
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public string BillNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateOnly DueDate { get; set; }
    public BillStatus Status { get; set; } = BillStatus.Unpaid;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
