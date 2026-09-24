namespace Limita.Business.DTOs.Transfer;

public class TransferRequestDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "Source account id must be greater than zero")]
    public int SourceAccountId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Beneficiary id must be greater than zero")]
    public int BeneficiaryId { get; set; }

    [Range(typeof(decimal), "0.01", "79228162514264337593543950335",
        ErrorMessage = "Amount must be greater than zero")]
    public decimal Amount { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }
}
