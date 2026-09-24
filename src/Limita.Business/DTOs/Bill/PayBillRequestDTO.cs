namespace Limita.Business.DTOs.Bill;

public class PayBillRequestDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "Source account id must be greater than zero")]
    public int SourceAccountId { get; set; }
}
