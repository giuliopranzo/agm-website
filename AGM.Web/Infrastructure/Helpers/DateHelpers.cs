using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace AGM.Web.Infrastructure.Helpers
{
    public class DateHelpers
    {
        public static DateTime StringToDate(string stringDate)
        {
            var cultureIt = CultureInfo.GetCultureInfo("it-IT");
            DateTime val;
            DateTime.TryParse(stringDate, cultureIt, DateTimeStyles.AssumeLocal, out val);
            return val;
        }
    }
}