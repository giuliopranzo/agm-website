using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AGM.Web.Infrastructure;

namespace AGM.Web.Models
{
    public class RetributionItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Month { get; set; }
        public RetributionItemType Type { get; set; }
        public int Qty { get; set; }
        public double Amount { get; set; }
        public double Total { get; set; }
    }
}