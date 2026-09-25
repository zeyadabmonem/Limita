using Limita.Data.Entities.Enums;

namespace Limita.Business.DTOs.Transactions;

public class TransactionResponseDTO
{

    public int Id { get; set; }
    public int AccountId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = string.Empty;

    public TransactionType Type { get; set; }

    public TransactionStatus Status { get; set; }

    public string Reference { get; set; } = string.Empty;


    public DateTime CreatedAt { get; set; }

    public string? BeneficiaryName { get; set; }
}
