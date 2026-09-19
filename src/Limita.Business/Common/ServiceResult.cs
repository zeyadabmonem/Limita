using System;
using System.Collections.Generic;
using System.Text;

namespace Limita.Business.Common
{
    public class ServiceResult<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
