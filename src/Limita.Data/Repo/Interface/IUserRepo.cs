using Limita.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Data.Repo.Interface
{
    public interface IUserRepo
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int userId);
        Task<bool> ExistsByEmailOrPhoneAsync(string email , string phone);
        Task<bool> ExistsByEmailOrPhoneForOtherUserAsync(int userId, string email, string phone);
        Task<int> AddUserAsync(User user);
        Task UpdateAsync(User user);
    }
}
