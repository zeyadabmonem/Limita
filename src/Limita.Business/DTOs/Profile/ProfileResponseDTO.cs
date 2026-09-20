namespace Limita.Business.DTOs.Profile;

public class ProfileResponseDTO
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? ProfileImage { get; set; }
    public string Language { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
}
