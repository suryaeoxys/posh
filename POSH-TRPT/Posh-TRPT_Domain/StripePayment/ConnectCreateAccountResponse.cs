using Newtonsoft.Json;
using Posh_TRPT_Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.StripePayment
{
	public class ConnectCreateAccountResponse
	{
		[JsonProperty("id")]
		public string? Id { get; set; }
		[JsonProperty("type")]
		public string? Type { get; set; }
		[JsonProperty("country")]
		public string? Country { get; set; }
		[JsonProperty("created")]
		public long? Created { get; set; }
		[JsonProperty("default_currency")]
		public string? Default_Currency { get; set; }
		[JsonProperty("email")]
		public string? Email { get; set; }
		[JsonProperty("external_accounts")]
		public External_Accounts? External_Accounts { get; set; }
		[JsonProperty("login_links")]
		public Login_Links? Login_Links { get; set; }
	}
	public class External_Accounts
	{
		[JsonProperty("url")]
		public string? URL { get; set; }
    }
	public class Login_Links
	{
		[JsonProperty("url")]
		public string? URL { get; set; }
	}
}
