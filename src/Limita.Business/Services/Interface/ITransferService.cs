using Limita.Business.Common;
using Limita.Business.DTOs.Transfer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Business.Services.Interface
{
    public interface ITransferService
    {
        Task<ServiceResult<TransferResponseDTO>> AddNewTransferAsync(int userId,TransferRequestDTO requestDTO);
        Task<ServiceResult<TransferResponseDTO>> GetTransferByIdAsync(int userId, int transactionId);
    }
}
