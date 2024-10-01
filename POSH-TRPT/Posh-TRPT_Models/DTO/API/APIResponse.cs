﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.API
{
    public class APIResponse<T>
    {
        public T? Data {    get; set; }
     
        public string? Message { get; set; }

        public HttpStatusCode Status {get; set; }
        public bool Success { get; set; }
        public CustomException? Error { get; set; }
    }
}
                     