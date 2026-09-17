namespace Limita.Data.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? ProfileImage { get; set; }
    public string Language { get; set; } = "en";
    public string Currency { get; set; } = "USD";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    public ICollection<Beneficiary> Beneficiaries { get; set; } = new List<Beneficiary>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
