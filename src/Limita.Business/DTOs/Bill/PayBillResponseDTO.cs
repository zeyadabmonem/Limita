namespace Limita.Business.DTOs.Bill;

public class PayBillResponseDTO
{
    public int BillId { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime PaidAt { get; set; }
    public string TransactionReference { get; set; } = string.Empty;
}