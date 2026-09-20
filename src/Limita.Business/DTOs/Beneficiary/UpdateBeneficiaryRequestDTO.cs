using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Limita.Business.DTOs.Beneficiary
{
    public class UpdateBeneficiaryRequestDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "AccountIdentifier is required")]
        public string AccountIdentifier { get; set; } = string.Empty;
    }
}
