namespace Limita.Business.DTOs.Cards;

public class AddCardRequestDTO
{
    [Range(1, int.MaxValue)]
    public int AccountId { get; set; }

    [Required]
    [StringLength(100)]
    public string CardName { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{4}$")]
    public string LastFourDigits { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Brand { get; set; } = string.Empty;

    public DateOnly ExpiryDate { get; set; }
}
