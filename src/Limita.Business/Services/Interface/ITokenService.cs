using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Business.Services.Interface
{
    public interface ITokenService
    {
        string CreateToken(int userId);
    }
}
