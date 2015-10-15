using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class Token
    {
        public string Id { get; set; }
        public bool IsConsumed { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}