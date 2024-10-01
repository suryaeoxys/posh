using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.RegisterDTO
{
    public class UserRegisterDTO
    {

        public string? Email { get; set; }
        public string? Password { get; set; }
        public IFormFile? ProfilePhoto { get; set; }
        public string? Name { get; set; }
        public DateTime? DOB { get; set; }

        public string? Mobile { get; set; }
        public string? Gender { get; set; }
    }
}
