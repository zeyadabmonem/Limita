using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Limita.Business.DTOs.Transfer
{
    public class TransferRequestDTO
    {
        [Required]
        public int SourceAccountId { get; set; }
        [Required]
        public int BeneficiaryId { get; set; }
        [Required]
        public decimal Amount { get; set; }

        public string? Note { get; set; }

    }
}
