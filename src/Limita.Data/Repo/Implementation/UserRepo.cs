using Limita.Data.Entities;
using Limita.Data.Repo.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Data.Repo.Implementation
{
    public class UserRepo : IUserRepo
    {
        private readonly LimitaDbContext dbContext;

        public UserRepo(LimitaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> ExistsByEmailOrPhoneAsync(string email, string phone)
        {
          

            return await dbContext.Users.AnyAsync(u => u.Email == email || u.PhoneNumber == phone); 
        }

        public async Task<int> AddUserAsync(User user)
        {
           await dbContext.Users.AddAsync(user);

            await dbContext.SaveChangesAsync();
            return user.Id;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await dbContext.Users.Where(u =>  u.Email == email).FirstOrDefaultAsync();
            return  user;
        }
    }
}
