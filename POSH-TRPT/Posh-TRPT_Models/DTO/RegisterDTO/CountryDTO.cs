﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.RegisterDTO
{
    public class CountryListDTO
    {
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? CountryCode { get; set; }
    }
}
