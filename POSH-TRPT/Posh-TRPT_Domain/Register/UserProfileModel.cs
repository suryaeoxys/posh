using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
	public class UserProfileModel
	{
        public string? UserId { get; set; }
		public string? Name { get; set; }
		public DateTime? Dob { get; set; }
        public IFormFile? ProfilePic { get; set; }
		public string? Email { get; set; }

	}
}
