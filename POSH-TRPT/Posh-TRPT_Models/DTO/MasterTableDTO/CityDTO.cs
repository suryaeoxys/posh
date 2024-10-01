using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.MasterTableDTO
{
    public class CityDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }

        public Guid? StateId { get; set; }
        public  StateDTO? State { get; set; }
    }
}
