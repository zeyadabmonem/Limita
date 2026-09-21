using Limita.Business.Common;
using Limita.Business.DTOs.Cards;
using Limita.Business.Services.Interface;
using Limita.Data.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Business.Services.Implementation
{
    public class CardService : ICardService
    {
        private readonly ICardRepo cardRepo;

        public CardService(ICardRepo cardRepo) 
        {
            this.cardRepo = cardRepo;
        }
        public async Task<ServiceResult<List<CardResponseDTO>>> GetAll(int userId)
        {

            var cards = await cardRepo.GetCardsByUserIdAsync(userId);

            var cardsReponse = new List<CardResponseDTO>();

            foreach (var card in cards)
            {
                cardsReponse.Add(new CardResponseDTO 
                {
                    Id = card.Id, Brand = card.Brand, CardName=card.CardName,
                 ExpiryDate = card.ExpiryDate, LastFourDigits = card.LastFourDigits, Status = card.Status,
                });
            }

            return new ServiceResult<List<CardResponseDTO>> {
                Message="Cards retrieved successfully", Success =true , Data = cardsReponse
            } ;
        }

        public async Task<ServiceResult<CardResponseDTO>> GetById(int userId, int cardId)
        {
            var card = await cardRepo.GetCardByIdAsync(cardId);

            if (card == null || card.Account.UserId != userId)
                return new ServiceResult<CardResponseDTO> { Success = false, Message = "Invalid operation" };

            CardResponseDTO responseDTO = new CardResponseDTO() 
            {
                 Id=card.Id,
                  Brand=card.Brand,
                   CardName =card.CardName,
                    ExpiryDate =card.ExpiryDate,
                     LastFourDigits = card.LastFourDigits,
                      Status = card.Status,
            };

            return new ServiceResult<CardResponseDTO> { Success = true, Message = "Card retrieved successfully", Data = responseDTO };
        }
    }
}
