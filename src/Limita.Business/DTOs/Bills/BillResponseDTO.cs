using Limita.Data.Entities.Enums;

namespace Limita.Business.DTOs.Bills
{
    public class BillResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ProviderName { get; set; }
        public string BillNumber { get; set; }
        public decimal Amount { get; set; }
        public DateOnly DueDate { get; set; }
        public BillStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
