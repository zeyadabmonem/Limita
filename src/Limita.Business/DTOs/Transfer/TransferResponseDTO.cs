using Limita.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Business.DTOs.Transfer
{
    public class TransferResponseDTO
    {
        public decimal Amount { get; set; }
        public string BeneficiaryName { get; set; } = string.Empty;
        public string TransactionReference { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public DateTime Date { get; set; } 
         public TransactionStatus Status { get; set; }

    }
}
