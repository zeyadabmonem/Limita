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
    public class LoginService : ILoginService
    {
        private readonly IUserRepo userRepo;
        private readonly ITokenService tokenService;

       public   LoginService(IUserRepo userRepo , ITokenService tokenService) {
            this.userRepo = userRepo;
            this.tokenService = tokenService;
        }

        public async Task<ServiceResult<LoginResponseDTO>> LoginAsync(LoginRequestDTO loginRequest)
        {
            User? user = await userRepo.GetByEmailAsync(loginRequest.Email);

            if (user == null)
                return new ServiceResult<LoginResponseDTO>() { Message = "Invalid Data", Success = false };

            bool isPasswordTrue = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash);

            if(!isPasswordTrue)
                return new ServiceResult<LoginResponseDTO>() { Message = "Invalid Data", Success = false };

            string token = tokenService.CreateToken(user.Id);

            LoginResponseDTO responseDTO = new LoginResponseDTO() { FullName = user.FullName , Id = user.Id, Token = token  };

            return new ServiceResult<LoginResponseDTO>() {  Data = responseDTO , Message = "Login successfully" , Success = true};

        }
    }
}
