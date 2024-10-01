using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Token
{
    public class TokenResponse
    {
        public string? TokenString { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
