using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.ResendVerification
{
    public class ResendVerificationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
