﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class ApiResponseError
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
    }
}