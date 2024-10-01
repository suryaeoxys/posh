using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.RegisterDTO
{
    public class ForgotPasswordVerifyCodeDTO
    {
        public string? Email { get; set; }
        public string? HashCode { get; set; }
        public string? Otp { get; set; }
        public string? DeviceId { get; set; }
    }
}
