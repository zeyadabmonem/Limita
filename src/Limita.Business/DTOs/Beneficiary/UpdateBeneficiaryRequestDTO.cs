namespace Limita.Business.DTOs.Beneficiary;

public class UpdateBeneficiaryRequestDTO
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Account identifier is required")]
    [StringLength(100)]
    public string AccountIdentifier { get; set; } = string.Empty;
}
