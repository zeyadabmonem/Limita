namespace Limita.Business.DTOs.Accounts;

public class AccountResponseDTO
{
    public int Id { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
