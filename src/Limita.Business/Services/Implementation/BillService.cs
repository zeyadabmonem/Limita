using Limita.Business.Common;
using Limita.Business.DTOs.Bills;
using Limita.Business.Services.Interface;
using Limita.Data.Entities;
using Limita.Data.Repo.Interface;

namespace Limita.Business.Services.Implementation
{
    public class BillService : IBillService
    {
        private readonly IBillRepo billRepo;

        public BillService(IBillRepo billRepo)
        {
            this.billRepo = billRepo;
        }

        ServiceResult<List<BillResponseDTO>> GetAll(int userId)
        {
            var bills = billRepo.GetAllBills(userId);

            var billsReponse = new List<BillResponseDTO>();

            foreach (var bill in bills)
            {
                billsReponse.Add(new BillResponseDTO
                {
                    Id = bill.Id,
                    UserId = bill.UserId,
                    ProviderName = bill.ProviderName,
                    BillNumber = bill.BillNumber,
                    Amount = bill.Amount,
                    DueDate = bill.DueDate,
                    Status = bill.Status,
                    CreatedAt = bill.CreatedAt,
                    PaidAt = bill.PaidAt
                });
            }

            return new ServiceResult<List<BillResponseDTO>>
            {
                Message = "Bills Retreived Successfully",
                Success = true,
                Data = billsReponse
            };

        }

        ServiceResult<List<BillResponseDTO>> IBillService.GetAll(int userId)
        {
            return GetAll(userId);
        }

        ServiceResult<BillResponseDTO> GetById(int userId , int billId)
        {
            var bills = billRepo.GetBillById(billId);

            var billResponse = (new BillResponseDTO 
            {
                Id = bills.Id,
                UserId = bills.UserId,
                ProviderName = bills.ProviderName,
                BillNumber = bills.BillNumber,
                Amount = bills.Amount,
                DueDate = bills.DueDate,
                Status = bills.Status,
                CreatedAt = bills.CreatedAt,
                PaidAt = bills.PaidAt
            });

            return new ServiceResult<BillResponseDTO>
            {
                Message = "Bill Retreived Successfully",
                Success = true,
                Data = billResponse
            };
        }

        ServiceResult<BillResponseDTO> IBillService.GetById(int userId, int billId)
        {
            return GetById(userId, billId);
        }
    }
}
