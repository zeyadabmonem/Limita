namespace Limita.Data.Entities;

public class Card
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string CardName { get; set; } = string.Empty;
    public string LastFourDigits { get; set; } = string.Empty; // exactly 4 digits, never full PAN/CVV
    public string Brand { get; set; } = string.Empty;
    public DateOnly ExpiryDate { get; set; }
    public CardStatus Status { get; set; } = CardStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Account Account { get; set; } = null!;
}
