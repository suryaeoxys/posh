using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.MasterTableDTO
{
    public class StateDTO
    {

        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? StateCode { get; set; }
        public CountryDTO? Country { get; set; }

        public Guid? CountryId { get; set; }
       
    }
}
