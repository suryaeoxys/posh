using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.CascadeDTO
{
    public class RegisterDTO
    {
        public RegisterDTO()
        {
            this.Countries = new List<SelectListItem>();
            this.States = new List<SelectListItem>();
            this.Cities = new List<SelectListItem>();
        }
        public List<SelectListItem> Countries { get;set; }
        public List<SelectListItem> States { get; set; }
        public List<SelectListItem> Cities { get; set; }
        public string? CountryId { get; set; }
        public string? StateId { get; set; }
        public string? CityId { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
    }
}
