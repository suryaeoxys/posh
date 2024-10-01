using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.TokenDTO
{
    public class TokenInfoUserDTO
    {
        public Guid? Id { get; set; }
        public string? UserName { get; set; }
        public string? RefreshToken { get; set; }
		public string? DeviceId { get; set; }
		public string? AccessToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public string? StripeCustId { get; set; }
        public bool PayoutStatus { get; set; } = false;
        public string? URL { get; set; } = string.Empty;
		public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
