using System.ComponentModel.DataAnnotations;

namespace Limita.Business.DTOs.Profile;

public class UpdateProfileRequestDTO
{
    [Required, StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(500)]
    public string? ProfileImage { get; set; }

    [Required, StringLength(10)]
    public string Language { get; set; } = string.Empty;

    [Required, StringLength(10)]
    public string Currency { get; set; } = string.Empty;
}
