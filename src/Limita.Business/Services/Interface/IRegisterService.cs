using Limita.Business.Common;
using Limita.Business.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Business.Services.Interface
{
    public interface IRegisterService
    {
        Task<ServiceResult<RegisterResponseDTO>>  RegisterAsync(RegisterRequestDTO request);
    }
}
