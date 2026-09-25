using Limita.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Business.DTOs.Cards
{
    public class CardResponseDTO
    {
        public int Id { get; set; }
        public string CardName { get; set; } = string.Empty;
        public string LastFourDigits { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public DateOnly ExpiryDate { get; set; }
        public CardStatus Status { get; set; } = CardStatus.Active;
    }
}
