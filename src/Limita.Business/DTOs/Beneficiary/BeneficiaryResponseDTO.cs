using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Limita.Business.DTOs.Beneficiary
{
    public class BeneficiaryResponseDTO
    {
        public  int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string AccountIdentifier { get; set; } = string.Empty;
    }
}
