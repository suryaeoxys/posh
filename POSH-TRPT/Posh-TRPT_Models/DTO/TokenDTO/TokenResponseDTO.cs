using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.TokenDTO
{
    public class TokenResponseDTO
    {
        public string? TokenString { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
