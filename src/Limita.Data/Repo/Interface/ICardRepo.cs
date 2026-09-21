using Limita.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Data.Repo.Interface
{
    public interface ICardRepo
    {
        Task<List<Card>> GetCardsByUserIdAsync(int userId);
        Task<Card?> GetCardByIdAsync(int cardId);


    }
}
