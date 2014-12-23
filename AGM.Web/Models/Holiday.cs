using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class Holiday
    {
        public int Id { get; set; }
        public string DateRaw { get; set; }

        public DateTime Date
        {
            get
            {
                var cultureIt = CultureInfo.GetCultureInfo("it-IT");
                DateTime val;
                DateTime.TryParse(DateRaw, cultureIt, DateTimeStyles.AssumeLocal, out val);
                return val;
            }
        }
    }
}