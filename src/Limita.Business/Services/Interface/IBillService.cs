using Limita.Business.Common;
using Limita.Business.DTOs.Bills;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Business.Services.Interface
{
    public interface IBillService
    {
        ServiceResult<List<BillResponseDTO>> GetAll(int userId);
        ServiceResult<BillResponseDTO> GetById(int userId, int billId);
    }
}
