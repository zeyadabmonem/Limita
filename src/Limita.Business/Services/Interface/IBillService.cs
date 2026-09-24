namespace Limita.Business.Services.Interface;

public interface IBillService
{
    Task<ServiceResult<List<BillResponseDTO>>> GetBillsAsync(int userId);
    Task<ServiceResult<BillResponseDTO>> GetBillByIdAsync(int userId, int billId);
    Task<ServiceResult<PayBillResponseDTO>> PayBillAsync(int userId, int billId, PayBillRequestDTO requestDTO);
}