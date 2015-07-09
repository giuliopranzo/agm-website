using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class Version
    {
        public string Code { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool UpdateSucceeded { get; set; }
        public DateTime LastUpdateTryDate { get; set; }
    }
}