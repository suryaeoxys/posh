using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
	public class Response
	{
		[JsonPropertyName("result")]
        public string? Result { get; set; }
    }
}
