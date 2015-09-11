using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using AGM.Web.Infrastructure.Helpers;

namespace AGM.Web.Models
{
    public class Festivity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsDeleted { get; set; }
    }
}