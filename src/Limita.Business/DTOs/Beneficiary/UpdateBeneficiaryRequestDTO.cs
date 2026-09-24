namespace Limita.Business.DTOs.Beneficiary;

public class UpdateBeneficiaryRequestDTO
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Account identifier is required")]
    public string AccountIdentifier { get; set; } = string.Empty;
}
