using Posh_TRPT_Models.DTO.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.RegisterDTO
{
    public class DriverRegisterResponseDTO
    {
        public string? Response { get; set; }
        public string? AccessToken { get; set; }
		public string? StripeCustId { get; set; }

		public string? UserId { get; set; }
    }
}
