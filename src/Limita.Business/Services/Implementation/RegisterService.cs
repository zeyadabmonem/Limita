using Azure;
using Limita.Business.Common;
using Limita.Business.DTOs.Auth;
using Limita.Business.Services.Interface;
using Limita.Data.Entities;
using Limita.Data.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Business.Services.Implementation
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepo userRepo;

        public RegisterService(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        public async Task<ServiceResult<RegisterResponseDTO>> RegisterAsync(RegisterRequestDTO request)
        {

            bool isExist = await userRepo.ExistsByEmailOrPhoneAsync(request.Email,request.PhoneNumber);
            if (isExist)
                return new ServiceResult<RegisterResponseDTO> { Success = false, Message = "Data is Invalid" };

            try
            {
                string passhash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                User user = new User
                {
                    PhoneNumber = request.PhoneNumber
                    ,
                    Email = request.Email
                    ,
                    PasswordHash = passhash
                    ,
                    CreatedAt = DateTime.UtcNow

                    ,
                    IsActive = true
                    ,
                    FullName = request.FullName

                };

                int id = await userRepo.AddUserAsync(user);

                RegisterResponseDTO response = new RegisterResponseDTO() { Id = id, FullName = request.FullName, Email = request.Email };

                return new ServiceResult<RegisterResponseDTO> { Data = response, Message = "User Created Successfully", Success = true };
            }
            catch (Exception)
            {
               return new ServiceResult<RegisterResponseDTO> {  Message = "Unexpected Error", Success = false };
            }
           
        }
    }
}
