using Limita.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Business.DTOs.Transaction
{
    public class TransactionFilterDTO
    {

        public TransactionType? Type { get; set; }
        public TransactionStatus? Status { get; set; }

        public DateTime? Date { get; set; }
    }
}
