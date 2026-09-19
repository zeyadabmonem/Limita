using Limita.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Data.Repo.Interface
{
    public interface IUserRepo
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailOrPhoneAsync(string email , string phone);
        Task<int> AddUserAsync(User user);
    }
}
