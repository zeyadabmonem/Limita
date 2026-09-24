using System.ComponentModel.DataAnnotations;

namespace Limita.Business.DTOs.Bill;

public class PayBillRequestDTO
{
    /// <summary>The account the bill amount is debited from.</summary>
    [Required]
    public int SourceAccountId { get; set; }
}