namespace Limita.Business.DTOs.Auth
{
    public class RegisterResponseDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

    }
}
