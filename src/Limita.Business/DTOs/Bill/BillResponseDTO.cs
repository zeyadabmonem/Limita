namespace Limita.Business.DTOs.Bill;

public class BillResponseDTO
{
    public int Id { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public string BillNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateOnly DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? PaidAt { get; set; }
}